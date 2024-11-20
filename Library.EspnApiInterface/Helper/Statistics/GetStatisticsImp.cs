using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace Library.EspnApiInterface.Helper.Statistics;

public class GetStatisticsImp : IGetStatistics
{
    public List<JObject>? GetStatistics(
        int? playerId,
        string? position,
        JObject? data)
    {
        return position switch
        {
            "QB" => GetQbStats(playerId, data),
            "RB" => GetRbStats(playerId, data),
            "WR" => GetWrStats(playerId, data),
            "TE" => GetTeStats(playerId, data),
            "D/ST" => GetDefenseStats(playerId, data),
            "K" => GetKickerStats(playerId, data),
            _ => []
        };
    }

    private List<JObject>? GetKickerStats(int? playerId, JObject? data)
    {
        return new List<JObject>();
    }

    private List<JObject>? GetDefenseStats(int? playerId, JObject? data)
    {
        return new List<JObject>();
    }

    private List<JObject>? GetTeStats(int? playerId, JObject? data)
    {
        return new List<JObject>();
    }

    private List<JObject>? GetWrStats(int? playerId, JObject? data)
    {
        return new List<JObject>();
    }

    private List<JObject>? GetRbStats(int? playerId, JObject? data)
    {
        return new List<JObject>();
    }

    private List<JObject>? GetQbStats(int? playerId, JObject? data)
    {
        return new List<JObject>();
    }
}