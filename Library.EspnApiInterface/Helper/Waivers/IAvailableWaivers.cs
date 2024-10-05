using Library.EspnApiInterface.DataModel;
using Newtonsoft.Json.Linq;

namespace Library.EspnApiInterface.Helper.Waivers;

public interface IAvailableWaivers
{
    List<JObject>? GetWaiverPlayers(JObject playersData);

    Task<IEnumerable<Player>> TopFiveRunningBacks();
}