using Library.EspnApiInterface.DataModel;
using Newtonsoft.Json.Linq;

namespace Library.EspnApiInterface.Helper;

public interface IAvailableWaivers
{
    List<JObject>? GetWaiverPlayers(JObject playersData);

    Task<IEnumerable<Player>> TopFiveRunningBacks();

    dynamic SetUpFilter();
}