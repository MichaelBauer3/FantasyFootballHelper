using Cocona;
using FantasyFootballHelper.Commands.CommandHelpers.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FantasyFootballHelper.Commands;

public class CommandRunner : CommandBase
{
    private readonly ILogger<CommandRunner> _logger;
    private readonly ISetUpApi _setupApi;
    private readonly IGetWaiverWirePlayers _getWaiverWirePlayers;
    private readonly IConfiguration _configuration;
    
    public CommandRunner(
        ILogger<CommandRunner> logger,
        ISetUpApi setUpApi,
        IGetWaiverWirePlayers getWaiverWirePlayers,
        IConfiguration configuration,
        ICoconaContextWrapper coconaContextWrapper) : base(coconaContextWrapper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _setupApi = setUpApi ?? throw new ArgumentNullException(nameof(setUpApi));
        _getWaiverWirePlayers = getWaiverWirePlayers ?? throw new ArgumentNullException(nameof(getWaiverWirePlayers));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    [Command(
        "command-runner",
        Description = "Calls all of the commands")]
    public async Task RunAsync()
    {
        _logger.LogInformation("Starting command runner");
        _logger.LogInformation("Setting up API");
        var returnedTuple  = await _setupApi.RunAsync(leagueId: _configuration["leagueId"],
            year: _configuration["year"],
            week: _configuration["week"]).ConfigureAwait(false);
        // TODO - Figure out how to replace the need for the "week" field. In its current state it needs to be updated every week
        var handler = returnedTuple.Item1;
        var endpoints = returnedTuple.Item2;
        _logger.LogInformation("Api setup complete");
        
        _logger.LogInformation("Getting waiver wire players");
        var players = await _getWaiverWirePlayers.RunAsync(handler, endpoints).ConfigureAwait(false);
        _logger.LogInformation("Waiver wire players complete");
        
        
        
        await Task.CompletedTask;
    }
}