using Library.EspnApiInterface.DataModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Library.EspnApiInterface.Helper.Players;

public class GetFantasyPlayersRosteredAndWaiverImp : IGetFantasyPlayersRosteredAndWaiver
{

    public List<JObject>? GetWaiverPlayers(JObject playersData)
    {
        var filteredPlayers = playersData["players"]?
            .Where(r => r["status"]?.ToString() == "WAIVERS" &&
                        (int?)r["player"]?["proTeamId"] != 0 &&
                        (int?)r["onTeamId"] == 0)
            .Select(r => new JObject
            {
                ["FirstName"] = r["player"]?["firstName"]?.ToString(),
                ["LastName"] = r["player"]?["lastName"]?.ToString(),
                ["PlayerId"] = r["player"]?["id"]?.ToString(),
                ["ProTeamId"] = r["player"]?["proTeamId"]?.ToString(),
                ["OnTeamId"] = (int?)r["onTeamId"],
                ["Position"] = EspnApiInterfaceImp.PositionsDictionary[(int?)r["player"]?["eligibleSlots"]?.Min() ?? -1]
            })
            .ToList();
        
        return filteredPlayers;
    }

    public List<JObject>? GetRosteredPlayers(JObject playersData)
    {
        var filteredPlayers = playersData["players"]?
            .Where(r => r["status"]?.ToString() == "ONTEAM" &&
                        (int?)r["onTeamId"] != 0)
            .Select(r => new JObject
            {
                ["FirstName"] = r["player"]?["firstName"]?.ToString(),
                ["LastName"] = r["player"]?["lastName"]?.ToString(),
                ["PlayerId"] = r["player"]?["id"]?.ToString(),
                ["ProTeamId"] = r["player"]?["proTeamId"]?.ToString(),
                ["OnTeamId"] = (int?)r["onTeamId"],
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