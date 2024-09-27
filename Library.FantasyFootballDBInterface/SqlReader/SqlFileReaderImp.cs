using Microsoft.Extensions.Logging;

namespace Library.FantasyFootballDBInterface.SqlReader;

public class SqlFileReaderImp : ISqlFileReader
{
    private readonly ILogger<SqlFileReaderImp> _logger;

    public SqlFileReaderImp(ILogger<SqlFileReaderImp> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task<string> GetSqlFileContentsAsync(string fileName, CancellationToken c)
    {
        // TODO - Make sure this works as intended
        fileName = $"{Directory.GetParent(fileName)}/{fileName}";
        string sql = string.Empty;
        DateTimeOffset readFileStartTime = DateTimeOffset.Now;
        bool success = false;
        
        try
        {
            using (StreamReader sr = new(File.OpenRead(fileName)))
            {
                sql = await sr.ReadToEndAsync();
            }
            success = true;
        }
        finally
        {
            _logger.LogInformation($"File {fileName} has been read." +
                                   $"\nIt took [{DateTimeOffset.Now - readFileStartTime}] seconds.]" +
                                   $"\nSuccess - [{success}]");
        }

        if (string.IsNullOrEmpty(sql))
        {
            throw new Exception($"File {fileName} not found");  
        }
        return sql;
    }
}