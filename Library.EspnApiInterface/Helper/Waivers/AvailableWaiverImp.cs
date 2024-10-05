using Library.EspnApiInterface.DataModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Library.EspnApiInterface.Helper.Waivers;

public class AvailableWaiverImp : IAvailableWaivers
{

    public List<JObject>? GetWaiverPlayers(JObject playersData)
    {
        var filteredPlayers = playersData["players"]?
            .Where(r => r["status"]?.ToString() == "WAIVERS" &&
                        (int?)r["player"]?["proTeamId"] != 0 &&
                        (int?)r["onTeamId"] == 0)
            .Select(r => new JObject
            {
                ["FirstName"] = r["player"]?["firstName"],
                ["LastName"] = r["player"]?["lastName"],
                ["PlayerId"] = r["player"]?["id"],
                ["ProTeamId"] = r["player"]?["proTeamId"],
                ["Position"] = EspnApiInterfaceImp.PositionsDictionary[(int?)r["player"]?["eligibleSlots"]?.Min() ?? -1]
            
            })
            .ToList();
        
        return filteredPlayers;
    }
    
    // TODO - Top 5 players for each position
    public async Task<IEnumerable<Player>> TopFiveRunningBacks()
    {

        await Task.Delay(5000);
        
        return new List<Player>();
    }
}