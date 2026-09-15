
from

https://github.com/ezzylearning/AI_Agents_In_CSharp_Dotnet

After modification to suit the PDF format.


thank you ezzylearning




run
win11
python3.12
ollama version is 0.34.1
Qwen3.8-27B-64k:latest ===Qwen3.8-27B-GSQ-RCO-IQ3_S-mtp:latest
I changed the name.



Rename the model name

PS C:\Users\Aytto\OneDrive\Desktop\AI_Agents_In_CSharp_Dotnet\AI_Agents_In_CSharp_Dotnet> Get-ChildItem -Recurse -Include *.cs, *.json, *.py | Select-String -Pattern "Qwen|mxbai|OLLAMA_MODEL_NAME" | Select-Object Path, LineNumber, Line

Path
----
C:\Users\Aytto\OneDrive\Desktop\AI_Agents_In_CSharp_Dotnet\AI_Agents_In_CSharp_Dotnet\Northwind.Api\bin\Debug\net10.0\appsettings.json
C:\Users\Aytto\OneDrive\Desktop\AI_Agents_In_CSharp_Dotnet\AI_Agents_In_CSharp_Dotnet\Northwind.Api\Properties\launchSettings.json
C:\Users\Aytto\OneDrive\Desktop\AI_Agents_In_CSharp_Dotnet\AI_Agents_In_CSharp_Dotnet\Northwind.Api\appsettings.json
C:\Users\Aytto\OneDrive\Desktop\AI_Agents_In_CSharp_Dotnet\AI_Agents_In_CSharp_Dotnet\Northwind.Infrastructure\AI\Agents\Ollama\Ollama...
C:\Users\Aytto\OneDrive\Desktop\AI_Agents_In_CSharp_Dotnet\AI_Agents_In_CSharp_Dotnet\Northwind.Infrastructure\AI\Tools\BookTools.cs
C:\Users\Aytto\OneDrive\Desktop\AI_Agents_In_CSharp_Dotnet\AI_Agents_In_CSharp_Dotnet\RAG_Book_Agent_Backup\CSharp_Files\appsettings.json
C:\Users\Aytto\OneDrive\Desktop\AI_Agents_In_CSharp_Dotnet\AI_Agents_In_CSharp_Dotnet\RAG_Book_Agent_Backup\CSharp_Files\BookTools.cs
C:\Users\Aytto\OneDrive\Desktop\AI_Agents_In_CSharp_Dotnet\AI_Agents_In_CSharp_Dotnet\RAG_Book_Agent_Backup\CSharp_Files\launchSetting...
C:\Users\Aytto\OneDrive\Desktop\AI_Agents_In_CSharp_Dotnet\AI_Agents_In_CSharp_Dotnet\RAG_Book_Agent_Backup\CSharp_Files\OllamaAgentFa...
C:\Users\Aytto\OneDrive\Desktop\AI_Agents_In_CSharp_Dotnet\AI_Agents_In_CSharp_Dotnet\RAG_Book_Agent_Backup\Scripts\ingest_book.py
C:\Users\Aytto\OneDrive\Desktop\AI_Agents_In_CSharp_Dotnet\AI_Agents_In_CSharp_Dotnet\ingest_book.py
