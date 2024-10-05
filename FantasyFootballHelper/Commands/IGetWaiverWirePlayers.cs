using Library.EspnApiInterface.DataModel;

namespace FantasyFootballHelper.Commands;

public interface IGetWaiverWirePlayers
{
    Task<IEnumerable<Player>> RunAsync(IEnumerable<string> endpoints);
}