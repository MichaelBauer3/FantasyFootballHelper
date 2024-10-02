using Cocona;
using FantasyFootballHelper.Commands.CommandHelpers.CommandRunnerHelper;
using FantasyFootballHelper.Commands.CommandHelpers.Generic;
using Library.FantasyFootballDBInterface;
using Library.FantasyFootballDBInterface.FantasyFootballDBMySqlInterface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FantasyFootballHelper.Commands;

public class CommandRunner : CommandBase
{
    private readonly ILogger<CommandRunner> _logger;
    private readonly ISetUpApi _setupApi;
    private readonly IGetWaiverWirePlayers _getWaiverWirePlayers;
    private readonly IFantasyFootballDbInterface _fantasyFootballDbInterface;
    private readonly ICommandRunnerHelper _commandRunnerHelper;
    private readonly IConfiguration _configuration;
    
    public CommandRunner(
        ILogger<CommandRunner> logger,
        ISetUpApi setUpApi,
        IGetWaiverWirePlayers getWaiverWirePlayers,
        IConfiguration configuration,
        IFantasyFootballDbInterface fantasyFootballDbInterface,
        ICommandRunnerHelper commandRunnerHelper,
        ICoconaContextWrapper coconaContextWrapper) : base(coconaContextWrapper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _setupApi = setUpApi ?? throw new ArgumentNullException(nameof(setUpApi));
        _getWaiverWirePlayers = getWaiverWirePlayers ?? throw new ArgumentNullException(nameof(getWaiverWirePlayers));
        _fantasyFootballDbInterface = fantasyFootballDbInterface ?? throw new ArgumentNullException(nameof(fantasyFootballDbInterface));
        _commandRunnerHelper = commandRunnerHelper ?? throw new ArgumentNullException(nameof(commandRunnerHelper));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    [Command(
        "command-runner",
        Description = "Calls all of the commands")]
    public async Task RunAsync()
    {
        _logger.LogInformation("Starting command runner");
        _logger.LogInformation("Setting up API");
        var year = _configuration["year"];
        var returnedTuple  = await _setupApi.RunAsync(leagueId: _configuration["leagueId"],
            year: year,
            week: _commandRunnerHelper.CalculateCurrentNflSeasonWeek(year)).ConfigureAwait(false);
        var handler = returnedTuple.Item1;
        var endpoints = returnedTuple.Item2;
        _logger.LogInformation("Api setup complete");
        
        _logger.LogInformation("Getting waiver wire players");
        var players = await _getWaiverWirePlayers.RunAsync(handler, endpoints).ConfigureAwait(false);
        var playersToInsert = players.ToList();
        if (playersToInsert.Any())
        {
            _logger.LogInformation($"[{playersToInsert.Count}] players are being inserted to the database");
            await _fantasyFootballDbInterface.InsertToMySqlDatabaseAsync(playersToInsert, "PLAYERS");
        }
        
        await Task.CompletedTask;
    }
}