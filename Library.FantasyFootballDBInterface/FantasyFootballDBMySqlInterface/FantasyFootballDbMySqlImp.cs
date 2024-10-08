using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Net.Sockets;
using Library.FantasyFootballDBInterface.SqlReader;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using MySql.Data.MySqlClient;

namespace Library.FantasyFootballDBInterface.FantasyFootballDBMySqlInterface;

public class FantasyFootballDbMySqlImp : IFantasyFootballDbMySqlInterface
{
    private readonly ISqlFileReader _sqlFileReader;
    private readonly ILogger<FantasyFootballDbMySqlImp> _logger;
    private readonly AsyncRetryPolicy _retryPolicy;

    public FantasyFootballDbMySqlImp(
        ISqlFileReader sqlFileReader,
        ILogger<FantasyFootballDbMySqlImp> logger
    )
    {
        _sqlFileReader = sqlFileReader ?? throw new ArgumentNullException(nameof(sqlFileReader));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _retryPolicy = Policy.Handle<HttpRequestException>()
            .Or<SocketException>()
            .WaitAndRetryAsync(
                retryCount: 10,
                retryAttempt => TimeSpan.FromSeconds(30 * retryAttempt),
                onRetry: (ex, waitTime) => _logger.LogWarning(ex, $"Retrying in {waitTime.TotalSeconds} seconds")
            );
    }

    public async Task<DataSet> RunQueryWithLiteralsToDataSetAsync(
        CommandType commandType,
        string sqlFileName,
        Dictionary<string, string> findReplaceLiteralParams,
        CancellationToken cancellationToken)
    {
        string sql = await _sqlFileReader.GetSqlFileContentsAsync(
            $"SQL/{sqlFileName}.sql",
            cancellationToken).ConfigureAwait(false);

        foreach (var findReplaceLiteralParam in findReplaceLiteralParams)
        {
            sql = sql.Replace(findReplaceLiteralParam.Key, findReplaceLiteralParam.Value);
        }

        _logger.LogInformation($"SQL file: {sqlFileName}.sql has successfully loaded");

        return await DownloadQueryWithParamsToDataSetAsync(
            commandType,
            sql,
            [],
            cancellationToken).ConfigureAwait(false);
    }

    public Task<DataSet> DownloadQueryWithParamsToDataSetAsync(
        CommandType commandType,
        string sql,
        MySqlParameter[] queryParams,
        CancellationToken cancellationToken)
    {
        return _retryPolicy.ExecuteAsync(() =>
        {
            DataSet ds = new DataSet();
            DateTimeOffset startTime = DateTimeOffset.Now;
            DateTimeOffset endTime;
            MySqlConnection? connection = null;
            bool success = false;

            try
            {
                connection =
                    new MySqlConnection(
                        "Server=localhost;Database=FantasyFootballDB;Username=root;Password=SimplepassWord10;"); // TODO - extract from secrets
                connection.Open();

                _logger.LogDebug("Connection opened");

                MySqlCommand command = connection.CreateCommand();
                command.CommandType = commandType;
                command.CommandText = sql;
                command.Parameters.Clear();

                foreach (var param in queryParams)
                {
                    command.Parameters.Add(param);
                }

                _logger.LogDebug($"{queryParams.Length} parameters added");

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                _logger.LogDebug($"Run 'Fill' starting at {DateTime.Now.ToString("MM/dd HH:mm:ss")}");
                adapter.Fill(ds);
                _logger.LogDebug($"Run 'Fill' ending at {DateTime.Now.ToString("MM/dd HH:mm:ss")}");
                endTime = DateTimeOffset.Now;

                command.Parameters.Clear();
                command.Dispose();
                connection.Close();

                success = true;
            }
            finally
            {
                if (null != connection)
                {
                    connection.Close();
                }
            }

            TimeSpan tsDifference = endTime - startTime;
            double tsSeconds = Math.Round(tsDifference.TotalSeconds, 3);
            _logger.LogDebug($"Success {success}: Query ran for [{tsSeconds}] seconds, and returned a DataTable with " +
                             $"[{ds.Tables[0].Rows.Count}] rows and [{ds.Tables[0].Columns.Count}] columns");

            return Task.FromResult(ds);
        });
    }

    public async Task InsertIntoTable<T>(
        IEnumerable<T> data,
        string tableName)
    {
        await _retryPolicy.ExecuteAsync(async () =>
        {
            using (var connection =
                   new MySqlConnection(
                       "Server=localhost;Database=FantasyFootballDB;Username=root;Password=SimplepassWord10"))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    var dataList = data.ToList();
                    var command = connection.CreateCommand();
                    await ClearTable(
                        tableName,
                        connection,
                        command);
                    foreach (var entity in dataList)
                    {
                        command = connection.CreateCommand();
                        command.Transaction = transaction;
                        command.CommandText = GenerateInsert(entity, tableName);

                        foreach (var property in typeof(T).GetProperties())
                        {
                            command.Parameters.AddWithValue($"@{property.Name}",
                                property.GetValue(entity) ?? DBNull.Value);
                        }
                        await command.ExecuteNonQueryAsync();
                    }

                    await transaction.CommitAsync();
                }
            }

            return Task.CompletedTask;
        });
    }

    public string GenerateInsert<T>(
        T entity,
        string tableName)
    {
        var properties = typeof(T).GetProperties();
        var columnNames = string.Join(", ", properties.Select(p => p.Name));
        var paramNames = string.Join(", ", properties.Select(p => $"@{p.Name}"));

        return $"INSERT INTO {tableName} ({columnNames}) VALUES ({paramNames})";
    }

    public async Task ClearTable(
        string tableName,
        MySqlConnection connection,
        MySqlCommand? command)
    {
        if (command != null)
        {
            command.CommandText = $"DELETE FROM {tableName}";
            await command.ExecuteNonQueryAsync();
        }
        else
        {
            // TODO - Make a exception for not having an established connection
        }
    }
}