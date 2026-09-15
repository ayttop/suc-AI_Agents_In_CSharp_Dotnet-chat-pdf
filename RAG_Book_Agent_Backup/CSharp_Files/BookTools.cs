using System.ComponentModel;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Northwind.Infrastructure.AI.Tools;

public sealed class BookTools(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("Northwind")!;
    private static readonly HttpClient _httpClient = new();

    [Description("البحث في نصوص وفقرات الكتاب للإجابة على أسئلة المستخدم.")]
    public async Task<string> SearchBookAsync(
        [Description("نص السؤال أو كلمات البحث عن موضوع محدد داخل الكتاب")] string query,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // 1. توليد الـ Vector لسؤال المستخدم عبر Ollama محلياً
            var embedReq = new { model = "mxbai-embed:latest", prompt = query };
            using var httpResp = await _httpClient.PostAsJsonAsync("http://localhost:11434/api/embeddings", embedReq, cancellationToken);
            httpResp.EnsureSuccessStatusCode();

            using var jsonDoc = await JsonDocument.ParseAsync(await httpResp.Content.ReadAsStreamAsync(cancellationToken), cancellationToken: cancellationToken);
            var queryVector = jsonDoc.RootElement.GetProperty("embedding")
                .EnumerateArray()
                .Select(x => x.GetSingle())
                .ToArray();

            // 2. جلب فقرات الكتاب من قاعدة البيانات وحساب التشابه الدلالي (Cosine Similarity)
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync(cancellationToken);

            using var cmd = new SqlCommand("SELECT PageNumber, ChunkText, Embedding FROM BookChunks", conn);
            using var reader = await cmd.ExecuteReaderAsync(cancellationToken);

            var scoredChunks = new List<(int Page, string Text, float Score)>();

            while (await reader.ReadAsync(cancellationToken))
            {
                int page = reader.GetInt32(0);
                string text = reader.GetString(1);
                string embJson = reader.GetString(2);

                var chunkVec = JsonSerializer.Deserialize<float[]>(embJson);
                if (chunkVec != null)
                {
                    float score = CosineSimilarity(queryVector, chunkVec);
                    scoredChunks.Add((page, text, score));
                }
            }

            if (scoredChunks.Count == 0)
                return "لم يتم العثور على أي معلومات داخل الكتاب.";

            // 3. أخذ أفضل 3 فقرات متطابقة دلالياً مع السؤال
            var topChunks = scoredChunks
                .OrderByDescending(c => c.Score)
                .Take(3)
                .Select(c => $"[من الصفحة {c.Page}]:\n{c.Text}");

            return string.Join("\n\n---\n\n", topChunks);
        }
        catch (Exception ex)
        {
            return $"خطأ أثناء البحث في الكتاب: {ex.Message}";
        }
    }

    private static float CosineSimilarity(float[] v1, float[] v2)
    {
        if (v1.Length != v2.Length) return 0f;
        float dot = 0f, mag1 = 0f, mag2 = 0f;
        for (int i = 0; i < v1.Length; i++)
        {
            dot += v1[i] * v2[i];
            mag1 += v1[i] * v1[i];
            mag2 += v2[i] * v2[i];
        }
        float mag = MathF.Sqrt(mag1) * MathF.Sqrt(mag2);
        return mag == 0f ? 0f : dot / mag;
    }
}
