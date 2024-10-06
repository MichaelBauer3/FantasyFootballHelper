using Library.EspnApiInterface.DataModel;
using Newtonsoft.Json.Linq;

namespace Library.EspnApiInterface.Helper.Players;

public interface IGetFantasyPlayersRosteredAndWaiver
{
    List<JObject>? GetWaiverPlayers(JObject playersData);
    
    List<JObject>? GetRosteredPlayers(JObject playersData);

    Task<IEnumerable<Player>> TopFiveRunningBacks();
}