using Cocona;
using FantasyFootballHelper.Commands.CommandHelpers.Generic;
using Library.EspnApiInterface;
using Library.EspnApiInterface.DataModel;
using Library.EspnApiInterface.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FantasyFootballHelper.Commands;

public class GetWaiverWirePlayersImp : IGetWaiverWirePlayers
{
    private readonly ILogger<GetWaiverWirePlayersImp> _logger;
    private readonly IConfiguration _configuration;
    private readonly IAvailableWaivers _availableWaivers;
    
    public GetWaiverWirePlayersImp(
        ILogger<GetWaiverWirePlayersImp> logger,
        IConfiguration configuration,
        IAvailableWaivers availableWaivers)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));    
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _availableWaivers = availableWaivers ?? throw new ArgumentNullException(nameof(availableWaivers));
    }
    
    [Command(
        "get-waiver-wire-players",
        Description = "Gathers players available on waiver wire")
    ]
    public async Task<IEnumerable<Player>> RunAsync(HttpClientHandler handler, IEnumerable<string> endpoints)
    {
        var filter = _availableWaivers.SetUpFilter();
        
        var players = new List<Player>();
        string jsonFilter = JsonConvert.SerializeObject(filter);
            
            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("X-Fantasy-Filter", jsonFilter);

                string url = endpoints.First(r => r.Contains("kona_player_info"));
                var response = await client.GetAsync(url).ConfigureAwait(false);
                
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var playersData = JObject.Parse(content);
                    
                    var filteredPlayers = _availableWaivers.GetWaiverPlayers(playersData);
                    
                    if (filteredPlayers is not null && filteredPlayers.Any())
                    {
                        foreach (var player in filteredPlayers)
                        {
                            players.Add(new Player()
                            {
                                FirstName = player["FirstName"]?.ToString(),
                                LastName = player["LastName"]?.ToString(),
                                PlayerId = (int?)player["PlayerId"],
                                ProTeamId = (int?)player["ProTeamId"],
                                Position = player["Position"]?.ToString(),
                            });
                        }
                    }
                }
                else
                {
                   _logger.LogError($"Error with status code: {response.StatusCode}");
                }
            }
            return players;
    }
}