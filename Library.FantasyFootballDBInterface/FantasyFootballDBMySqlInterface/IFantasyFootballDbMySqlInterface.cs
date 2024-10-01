using System.Data;
using MySql.Data.MySqlClient;

namespace Library.FantasyFootballDBInterface.FantasyFootballDBMySqlInterface;

public interface IFantasyFootballDbMySqlInterface
{
    Task<DataSet> RunQueryWithLiteralsToDataSetAsync(
        CommandType commandType,
        string sqlFileName,
        Dictionary<string, string> findReplaceLiteralParams,
        CancellationToken cancellationToken);

    Task<DataSet> DownloadQueryWithParamsToDataSetAsync(
        CommandType commandType,
        string sql,
        MySqlParameter[] queryParams,
        CancellationToken cancellationToken);

    string GenerateInsert<T>(
        T entity,
        string tableName);

    Task InsertIntoTable<T>(
        IEnumerable<T> data,
        string tableName);

    Task ClearTable(
        string tableName,
        MySqlConnection connection,
        MySqlCommand? command);
}