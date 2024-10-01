using System.Text;
using Library.FantasyFootballDBInterface.FantasyFootballDBMySqlInterface;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace Library.FantasyFootballDBInterface;

public class FantasyFootballDbInterfaceImp : IFantasyFootballDbInterface
{
    private readonly IFantasyFootballDbMySqlInterface _fantasyFootballDb;
    private readonly ILogger<FantasyFootballDbInterfaceImp> _logger;

    public FantasyFootballDbInterfaceImp(
        IFantasyFootballDbMySqlInterface fantasyFootballDb,
        ILogger<FantasyFootballDbInterfaceImp> logger
        )
    {
        _fantasyFootballDb = fantasyFootballDb ?? throw new ArgumentNullException(nameof(fantasyFootballDb));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task WriteListToFileAsync<T>(
        List<T> downloadList,
        string downloadFileName,
        bool hasHeaderRow,
        string delimiter,
        string textQualifier,
        bool makeExtractWhenNoData,
        CancellationToken c)
    {
        await WriteListToFileAsync<T>(
            downloadList,
            downloadFileName,
            hasHeaderRow,
            delimiter,
            textQualifier,
            makeExtractWhenNoData,
            false,
            c).ConfigureAwait(false);
    }

    public async Task WriteListToFileAsync<T>(
        List<T> downloadList,
        string downloadFileName,
        bool hasHeaderRow,
        string delimiter,
        string textQualifier,
        bool makeExtractWhenNoData,
        bool shouldAppendToExistingFile,
        CancellationToken c)
    {
        // TODO - Add a more sophisticated path / way to export data
        downloadFileName = Path.Combine(Directory.GetCurrentDirectory(), downloadFileName);
        delimiter = delimiter ?? string.Empty;
        textQualifier = textQualifier ?? string.Empty;
        var output = new StringBuilder();
        var fields = new Collection<string>();
        Type elementType = typeof(T);
        
        _logger.LogInformation($"Output data in type List<T> to output file [{downloadFileName}].");

        if (File.Exists(downloadFileName) && !shouldAppendToExistingFile)
        {
            _logger.LogInformation($"File [{downloadFileName}] already exists and shouldAppendToExistingFile is false, " +
                                   $" now deleting the file.");
            File.Delete(downloadFileName);
        }
        
        hasHeaderRow = (
            hasHeaderRow && 
            shouldAppendToExistingFile &&
            File.Exists(downloadFileName) ? false : hasHeaderRow);

        using (var writerOutputData = new StreamWriter(downloadFileName, shouldAppendToExistingFile))
        {
            if (hasHeaderRow)
            {
                foreach (var propInfo in elementType.GetProperties())
                {
                    string columnName = propInfo.Name;
                    fields.Add(string.Format("{0}{1}{0}", textQualifier, columnName));
                }
                
                await writerOutputData.WriteLineAsync(string.Join(delimiter, fields.ToArray())).ConfigureAwait(false);
                fields.Clear();
                hasHeaderRow = false;
            }

            foreach (T record in downloadList)
            {
                fields.Clear();

                foreach (var propInfo in elementType.GetProperties())
                {
                    if ((propInfo.GetValue(record, null) ?? DBNull.Value) is string)
                    {
                        fields.Add(string.Format("{0}{1}{0}", textQualifier, propInfo.GetValue(record, null) ?? DBNull.Value));
                    }
                    else
                    {
                        fields.Add((propInfo.GetValue(record, null) ?? DBNull.Value).ToString() ?? string.Empty);
                    }
                    
                    await writerOutputData.WriteLineAsync(string.Join(delimiter, fields.ToArray())).ConfigureAwait(false);
                }
            }
        }

        if (!makeExtractWhenNoData && downloadList.Count == 0)
        {
            _logger.LogInformation($"No records were made to the download file.");
            if (File.Exists(downloadFileName))
                File.Delete(downloadFileName);
        }
    }

    public async Task InsertToMySqlDatabaseAsync<T>(
        List<T> downloadList,
        string sqlQuery)
    {
        await _fantasyFootballDb.InsertIntoTable(downloadList, sqlQuery);
    }
}