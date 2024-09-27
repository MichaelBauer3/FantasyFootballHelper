using System.Data;

namespace Library.FantasyFootballDBInterface.FantasyFootballDBMySqlInterface;

public interface IFantasyFootballDbMySqlInterface
{
    Task<DataSet> RunQueryWithLiteralsToDataSetAsync(
        CommandType commandType,
        string sqlFileName,
        Dictionary<string, string> findReplaceLiteralParams,
        CancellationToken cancellationToken);
}