IF NOT EXISTS (SELECT 1 FROM sysobjects WHERE name='BookChunks' AND xtype='U')
BEGIN
    CREATE TABLE BookChunks (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        PageNumber INT,
        ChunkText NVARCHAR(MAX),
        Embedding NVARCHAR(MAX)
    );
END
