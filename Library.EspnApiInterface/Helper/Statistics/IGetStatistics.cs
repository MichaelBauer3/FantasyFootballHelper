using Newtonsoft.Json.Linq;

namespace Library.EspnApiInterface.Helper.Statistics;

public interface IGetStatistics
{
    public List<JObject>? GetStatistics(
        int? playerId,
        string? position,
        JObject? data);
}