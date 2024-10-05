using Cocona;

namespace FantasyFootballHelper.Commands;

public interface ISetUpApi
{
    Task<IEnumerable<string>> RunAsync(
        [Option("leagueId")] string? leagueId = null,
        [Option("year")] string? year = null,
        [Option("playerId")] string? playerId = null,
        [Option("team")] string? team = null,
        [Option("week")] string? week = null);
}