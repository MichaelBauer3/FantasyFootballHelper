using Newtonsoft.Json.Linq;

namespace Library.EspnApiInterface.Helper.FantasyTeams;

public interface IFantasyTeamsFromLeague
{
    List<JObject>? GetTeamIdentifiersAndStats(JObject leagueData);

    string GetTeamName(JToken r, JObject leagueData);

    decimal? GetPointsFor(JToken r, JObject leagueData);

    decimal? GetPointsAgainst(JToken r, JObject leagueData);

    int? GetWins(JToken r, JObject leagueData);

    int? GetLosses(JToken r, JObject leagueData);

    int? GetLeagueTeamId(JToken r, JObject leagueData);
}