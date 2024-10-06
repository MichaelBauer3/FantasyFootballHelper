using Library.EspnApiInterface.DataModel;

namespace FantasyFootballHelper.Commands;

public interface IGetRosteredPlayers
{
    Task<IEnumerable<Player>> RunAsync(IEnumerable<string> endpoints);
}