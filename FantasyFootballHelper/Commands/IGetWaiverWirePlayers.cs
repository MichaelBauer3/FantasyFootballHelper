using Library.EspnApiInterface.DataModel;

namespace FantasyFootballHelper.Commands;

public interface IGetWaiverWirePlayers
{
    Task<IEnumerable<Player>> RunAsync(HttpClientHandler handler, IEnumerable<string> endpoints);
}