namespace Library.FantasyFootballDBInterface;

public interface IFantasyFootballDbInterface
{
    Task WriteListToFileAsync<T>(
        List<T> downloadList,
        string downloadFileName,
        bool hasHeaderRow,
        string delimiter,
        string textQualifier,
        bool makeExtractWhenNoData,
        bool shouldAppendToExistingFile,
        CancellationToken c);

    Task WriteListToFileAsync<T>(
        List<T> downloadList,
        string downloadFileName,
        bool hasHeaderRow,
        string delimiter,
        string textQualifier,
        bool makeExtractWhenNoData,
        CancellationToken c);

    Task InsertToMySqlDatabaseAsync<T>(
        List<T> downloadList,
        string sqlQuery);
}