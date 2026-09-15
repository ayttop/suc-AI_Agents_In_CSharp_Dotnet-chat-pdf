using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Northwind.Infrastructure.AI.Tools;
using OllamaSharp;

namespace Northwind.Infrastructure.AI.Agents.Ollama
{
    public sealed class OllamaAgentFactory(BookTools bookTools)
    {
        public AIAgent CreateCustomerSupportAgent()
        {
            var endpoint = Environment.GetEnvironmentVariable("OLLAMA_ENDPOINT") ?? "http://localhost:11434";
            var modelName = Environment.GetEnvironmentVariable("OLLAMA_MODEL_NAME") ?? "Qwen3.8-27B-64k:latest";

            var client = new OllamaApiClient(new Uri(endpoint), modelName);

            var agent = client.AsAIAgent(
                name: "BookAssistantAgent",
                description: "وكيل ذكي مخصص للإجابة عن أسئلة محتوى الكتاب والملفات المرفوعة بدقة.",
                instructions:
                    """
                    أنت مساعد وخبير ذكي في محتوى الكتاب المرفوع.
                    مهمتك الإجابة على أسئلة واستفسارات المستخدم بناءً على محتوى الكتاب فقط.
                    
                    القواعد:
                    1. استخدم دائماً أداة SearchBookAsync للبحث في محتوى الكتاب قبل أن تجيب.
                    2. لا تخترع أي معلومات غير موجودة في نتائج البحث.
                    3. اذكر رقم الصفحة إن وجد في نتيجة البحث لتوثيق الإجابة.
                    4. أجب بنفس لغة سؤال المستخدم (عربي أو إنجليزي).
                    """,
                tools:
                [
                    AIFunctionFactory.Create(bookTools.SearchBookAsync)
                ]);

            return agent;
        }
    }
}
