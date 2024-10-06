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
    private readonly IGetRosteredPlayers _getRosteredPlayers;
    private readonly IGetFantasyTeams _getFantasyTeams;
    private readonly IFantasyFootballDbInterface _fantasyFootballDbInterface;
    private readonly ICommandRunnerHelper _commandRunnerHelper;
    private readonly IConfiguration _configuration;
    
    public CommandRunner(
        ILogger<CommandRunner> logger,
        ISetUpApi setUpApi,
        IGetWaiverWirePlayers getWaiverWirePlayers,
        IGetRosteredPlayers getRosteredPlayers,
        IGetFantasyTeams getFantasyTeams,
        IConfiguration configuration,
        IFantasyFootballDbInterface fantasyFootballDbInterface,
        ICommandRunnerHelper commandRunnerHelper,
        ICoconaContextWrapper coconaContextWrapper) : base(coconaContextWrapper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _setupApi = setUpApi ?? throw new ArgumentNullException(nameof(setUpApi));
        _getWaiverWirePlayers = getWaiverWirePlayers ?? throw new ArgumentNullException(nameof(getWaiverWirePlayers));
        _getRosteredPlayers = getRosteredPlayers ?? throw new ArgumentNullException(nameof(getRosteredPlayers));
        _getFantasyTeams = getFantasyTeams ?? throw new ArgumentNullException(nameof(getFantasyTeams));
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
        var endpoints = await _setupApi.RunAsync(leagueId: _configuration["leagueId"],
            year: year,
            week: _commandRunnerHelper.CalculateCurrentNflSeasonWeek(year)).ConfigureAwait(false);
        endpoints = endpoints.Select(endpoint => endpoint.ToString()).ToList();
        _logger.LogInformation("Api setup complete");
        
        _logger.LogInformation("Getting waiver wire players and rostered players");
        var wirePlayers = await _getWaiverWirePlayers.RunAsync(endpoints).ConfigureAwait(false);
        var rosteredPlayers = await _getRosteredPlayers.RunAsync(endpoints).ConfigureAwait(false);
        
        var playersToInsert = wirePlayers.Union(rosteredPlayers).ToList();
        if (playersToInsert.Any())
        {
            _logger.LogInformation($"[{playersToInsert.Count}] players are being inserted to the database");
            await _fantasyFootballDbInterface.InsertToMySqlDatabaseAsync(playersToInsert, "PLAYERS");
        }
        
        _logger.LogInformation("Getting fantasy teams information");
        var teams = await _getFantasyTeams.RunAsync(endpoints).ConfigureAwait(false);
        foreach (var team in teams)
        {
            var playersForTeam = playersToInsert.Where(player => player.OnTeamId == team.TeamId).ToList();
            team.Roster?.AddRange(playersForTeam);
        }
    }
}