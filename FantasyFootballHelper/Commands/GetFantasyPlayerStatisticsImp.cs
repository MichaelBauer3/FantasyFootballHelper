using System.Collections.Immutable;
using Cocona;
using Library.EspnApiInterface.DataModel;
using Library.EspnApiInterface.Helper;
using Library.EspnApiInterface.Helper.Statistics;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FantasyFootballHelper.Commands;

public class GetFantasyPlayerStatisticsImp : IGetFantasyPlayerStatistics
{
    private readonly IGetStatistics _getStatistics;
    private readonly IEspnApiCall _espnApiCall;
    private readonly HttpClient _httpClient;
    private readonly ILogger<GetFantasyPlayerStatisticsImp> _logger;

    public GetFantasyPlayerStatisticsImp(
        IGetStatistics getStatistics,
        IEspnApiCall espnApiCall,
        HttpClient httpClient,
        ILogger<GetFantasyPlayerStatisticsImp> logger)
    {
        _getStatistics = getStatistics ?? throw new ArgumentNullException(nameof(getStatistics));
        _espnApiCall = espnApiCall ?? throw new ArgumentNullException(nameof(espnApiCall));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    [Command(
        "get-fantasy-player-statistics",
        Description = "Gets fantasy player statistics.")]
    public async Task<ImmutableDictionary<int, PlayerStats>> RunAsync(List<Player>? playersToInsert, IEnumerable<string> endpoints)
    {
        var playerStatsDict = ImmutableDictionary<int, PlayerStats>.Empty;
        if (playersToInsert is null)
        {
            return playerStatsDict;
        }
        
        var filter = _espnApiCall.SetUpFilter();
        
        var teams = new List<FantasyTeam>();
        string jsonFilter = JsonConvert.SerializeObject(filter);
        
        _httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        _httpClient.DefaultRequestHeaders.Add("X-Fantasy-Filter", jsonFilter);

        string url = endpoints.First(r => r.Contains("mTeam"));
        var response = await _httpClient.GetAsync(url).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            var playerStatData = JObject.Parse(content);
            
            foreach (var player in playersToInsert)
            {
                var playerStats = _getStatistics.GetStatistics(player.PlayerId, player.Position, playerStatData);
            
            
                //playerStatsDict.Add(Convert.ToInt32(player.PlayerId), playerStats);
            }
        }
        
        return playerStatsDict;
    }
}