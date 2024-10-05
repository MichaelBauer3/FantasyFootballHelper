using Library.EspnApiInterface.DataModel;

namespace FantasyFootballHelper.Commands;

public interface IGetFantasyTeams
{
    Task<IEnumerable<FantasyTeam>> RunAsync(IEnumerable<string> endpoints);
}