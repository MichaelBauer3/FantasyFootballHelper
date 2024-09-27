namespace Library.FantasyFootballDBInterface.SqlReader;

public interface ISqlFileReader
{
    Task<string> GetSqlFileContentsAsync(string fileName, CancellationToken c);
}