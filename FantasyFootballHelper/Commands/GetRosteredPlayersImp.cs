using Cocona;
using FantasyFootballHelper.Commands.CommandHelpers.Generic;
using Library.EspnApiInterface;
using Library.EspnApiInterface.DataModel;
using Library.EspnApiInterface.Helper;
using Library.EspnApiInterface.Helper.Players;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace FantasyFootballHelper.Commands;

public class GetRosteredPlayersImp : IGetRosteredPlayers
{
    private readonly ILogger<GetRosteredPlayersImp> _logger;
    private readonly IConfiguration _configuration;
    private readonly IGetFantasyPlayersRosteredAndWaiver _getFantasyPlayersRosteredAndWaiver;
    private readonly IEspnApiCall _espnApiCall;
    private readonly HttpClient _httpClient;

    public GetRosteredPlayersImp(
        ILogger<GetRosteredPlayersImp> logger,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        IGetFantasyPlayersRosteredAndWaiver getFantasyPlayersRosteredAndWaiver,
        IEspnApiCall espnApiCall)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpClient = httpClientFactory.CreateClient("SharedHttpClient") ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _getFantasyPlayersRosteredAndWaiver = getFantasyPlayersRosteredAndWaiver ?? throw new ArgumentNullException(nameof(getFantasyPlayersRosteredAndWaiver));
        _espnApiCall = espnApiCall ?? throw new ArgumentNullException(nameof(espnApiCall));
    }

    [Command(
            "get-rostered-players",
            Description = "Gathers players that are rostered")
    ]
    public async Task<IEnumerable<Player>> RunAsync(IEnumerable<string> endpoints)
    {
        var filter = _espnApiCall.SetUpFilter();
    
        var players = new List<Player>();
        string jsonFilter = JsonConvert.SerializeObject(filter);
    
        _httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        _httpClient.DefaultRequestHeaders.Add("X-Fantasy-Filter", jsonFilter);

        string url = endpoints.First(r => r.Contains("kona_player_info"));
        var response = await _httpClient.GetAsync(url).ConfigureAwait(false);
    
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            var playersData = JObject.Parse(content);
        
            var filteredPlayers = _getFantasyPlayersRosteredAndWaiver.GetRosteredPlayers(playersData);
        
            if (filteredPlayers is not null && filteredPlayers.Any())
            {
                foreach (var player in filteredPlayers)
                {
                    players.Add(new Player()
                    {
                        Name = player["FirstName"] + " " + player["LastName"],
                        PlayerId = (int?)player["PlayerId"],
                        ProTeamId = (int?)player["ProTeamId"],
                        OnTeamId = (int?)player["OnTeamId"],
                        Position = player["Position"]?.ToString(),
                    });
                }
            }
        }
        else
        {
            _logger.LogError($"Error with status code: {response.StatusCode}");
        }
        return players;
    } 
}
