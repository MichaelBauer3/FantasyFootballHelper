using Cocona;
using Library.EspnApiInterface.DataModel;
using Library.EspnApiInterface.Helper;
using Library.EspnApiInterface.Helper.FantasyTeams;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FantasyFootballHelper.Commands;

public class GetFantasyTeamsImp : IGetFantasyTeams
{
    
    private readonly IFantasyTeamsFromLeague _fantasyTeamsFromLeague;
    private readonly IEspnApiCall _espnApiCall;
    private readonly ILogger<GetFantasyTeamsImp> _logger;
    private readonly HttpClient _httpClient;
    
    public GetFantasyTeamsImp(
        IFantasyTeamsFromLeague fantasyTeamsFromLeague,
        IEspnApiCall espnApiCall,
        ILogger<GetFantasyTeamsImp> logger,
        IHttpClientFactory httpClientFactory
        )
    {
        _fantasyTeamsFromLeague = fantasyTeamsFromLeague ?? throw new ArgumentNullException(nameof(fantasyTeamsFromLeague));
        _espnApiCall = espnApiCall ?? throw new ArgumentNullException(nameof(espnApiCall));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpClient = httpClientFactory.CreateClient("SharedHttpClient") ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }
    
    [Command(
        "get-fantasy-teams",
        Description = "Gets fantasy teams from the league.")]
    public async Task<IEnumerable<FantasyTeam>> RunAsync(IEnumerable<string> endpoints)
    {
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
            var leagueData = JObject.Parse(content);
                
            var filteredTeams = _fantasyTeamsFromLeague.GetTeamIdentifiersAndStats(leagueData);
                
            if (filteredTeams is not null && filteredTeams.Any())
            {
                foreach (var team in filteredTeams)
                {
                    teams.Add(new FantasyTeam()
                    {
                        TeamName = team["TeamName"]?.ToString(),
                        TeamOwnerId = team["TeamOwnerId"]?.ToString(),
                        TeamId = (int?)team["LeagueTeamId"],
                        PointsFor = (decimal?)team["PointsFor"],
                        PointsAgainst = (decimal?)team["PointsAgainst"],
                        Wins = (int?)team["Wins"],
                        Losses = (int?)team["Losses"]
                    });
                }
            }
        }
        else
        {
            _logger.LogError($"Error with status code: {response.StatusCode}");
        }
        
        return teams;
    }
}