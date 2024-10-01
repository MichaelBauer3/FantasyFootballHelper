using Cocona;
using FantasyFootballHelper.Commands.CommandHelpers.Generic;
using FantasyFootballHelper.Commands.CommandHelpers.SetUpApiHelper;
using Library.EspnApiInterface.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Library.EspnApiInterface;

namespace FantasyFootballHelper.Commands;

public class SetUpApiImp : ISetUpApi
{
    private readonly ILogger<SetUpApiImp> _logger;
    private readonly IEspnApiCall _espnApiCall;
    private readonly ISetUpApiHelper _setUpApiHelper;
    
    public SetUpApiImp(
        IEspnApiCall espnApiCall,
        ILogger<SetUpApiImp> logger,
        ISetUpApiHelper getDataAndSendToMySql)
    {
        _espnApiCall = espnApiCall ?? throw new ArgumentNullException(nameof(espnApiCall));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _setUpApiHelper = getDataAndSendToMySql ?? throw new ArgumentNullException(nameof(getDataAndSendToMySql));
    }

    [Command(
        "set-up-api",
        Description = "configures the api's"
    )]
    public async Task<(HttpClientHandler, IEnumerable<string>)> RunAsync(
        [Option("leagueId")] string? leagueId = null,
        [Option("year")] string? year = null,
        [Option("playerId")] string? playerId = null,
        [Option("team")] string? team = null,
        [Option("week")] string? week = null)
    {
        var placeholders = new Dictionary<string, string>
        {
            { "{year}", year ?? string.Empty },
            { "{leagueId}", leagueId ?? string.Empty },
            { "{playerId}", playerId ?? string.Empty },
            { "{teamId}", $"{EspnApiInterfaceImp.TeamDictionary[team ?? string.Empty].TeamId}" },
            { "{week}", week ?? string.Empty},
        };
        
        _logger.LogInformation("Setting up api");
        var endpoints = _setUpApiHelper.GetApiEndPoints(placeholders);
        var cookieJar = _espnApiCall.SetUpEspnApiCookies();
        HttpClientHandler handler = new HttpClientHandler
        {
            CookieContainer = cookieJar
        };
        _logger.LogInformation("Api setup complete");

        await Task.CompletedTask;
        return (handler, endpoints);
    }
}