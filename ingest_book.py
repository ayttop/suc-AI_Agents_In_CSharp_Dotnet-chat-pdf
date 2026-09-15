import json
import pyodbc
from pypdf import PdfReader
import requests

PDF_PATH = r"1.pdf"

# تجاوز أي بروكسي أو VPN في الويندوز للاتصال المحلي
session = requests.Session()
session.trust_env = False

# 1. فحص الاتصال بـ Ollama
try:
    res = session.get("http://127.0.0.1:11434", timeout=5)
    print("🟢 خادم Ollama متصل ويعمل بنجاح:", res.text.strip())
except Exception as e:
    print(f"🔴 تعذر الاتصال بـ Ollama: {e}")
    exit(1)

# 2. الاتصال بـ SQL Server والتأكد من وجود الجدول
try:
    conn = pyodbc.connect(
        "DRIVER={ODBC Driver 17 for SQL Server};"
        "SERVER=localhost;"
        "DATABASE=Northwind;"
        "Trusted_Connection=yes;"
    )
    cursor = conn.cursor()
    cursor.execute("""
        IF NOT EXISTS (SELECT 1 FROM sysobjects WHERE name='BookChunks' AND xtype='U')
        CREATE TABLE BookChunks (
            Id INT IDENTITY(1,1) PRIMARY KEY,
            PageNumber INT,
            ChunkText NVARCHAR(MAX),
            Embedding NVARCHAR(MAX)
        );
    """)
    conn.commit()
    print("🟢 تم الاتصال بقاعدة البيانات والتحقق من جدول BookChunks بنجاح!")
except Exception as e:
    print(f"🔴 خطأ في قاعدة البيانات: {e}")
    exit(1)

print(f"📖 جاري قراءة وتحليل الكتاب: {PDF_PATH}")
reader = PdfReader(PDF_PATH)
print(f"📄 إجمالي عدد الصفحات: {len(reader.pages)}")

chunk_size = 700
overlap = 100
total_chunks = 0

for page_idx, page in enumerate(reader.pages):
    text = page.extract_text()
    if not text or not text.strip():
        continue

    start = 0
    while start < len(text):
        chunk = text[start:start + chunk_size].strip()
        if len(chunk) > 50:
            try:
                resp = session.post(
                    "http://127.0.0.1:11434/api/embeddings",
                    json={"model": "mxbai-embed:latest", "prompt": chunk},
                    timeout=30
                )
                if resp.status_code == 200:
                    emb = resp.json()["embedding"]
                    cursor.execute("""
                        INSERT INTO BookChunks (PageNumber, ChunkText, Embedding)
                        VALUES (?, ?, ?)
                    """, (page_idx + 1, chunk, json.dumps(emb)))
                    total_chunks += 1
                else:
                    print(f"⚠️ تنبيه صفحة {page_idx + 1}: {resp.text}")
            except Exception as ex:
                print(f"⚠️ خطأ في فقرة: {ex}")

        start += chunk_size - overlap

    conn.commit()
    print(f"✅ تم حفظ الصفحة {page_idx + 1} من {len(reader.pages)}")

cursor.close()
conn.close()
print(f"\n🎉 مبروك! تم إدخال الكتاب بالكامل في قاعدة البيانات بنجاح بإجمالي {total_chunks} فقرة!")
