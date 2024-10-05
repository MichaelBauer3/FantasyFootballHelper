using Newtonsoft.Json.Linq;

namespace Library.EspnApiInterface.Helper.FantasyTeams;

public class FantasyTeamsFromLeagueImp : IFantasyTeamsFromLeague
{
    public List<JObject>? GetTeamIdentifiersAndStats(JObject leagueData)
    {
        var filteredTeams = leagueData["members"]?
            .Select(r => new JObject
            {
                ["FirstName"] = r["firstName"],
                ["LastName"] = r["lastName"],
                ["TeamOwnerId"] = r["id"],
                ["LeagueTeamId"] = GetLeagueTeamId(r, leagueData),
                ["TeamName"] = GetTeamName(r, leagueData),
                ["PointsFor"] = GetPointsFor(r, leagueData),
                ["PointsAgainst"] = GetPointsAgainst(r, leagueData),
                ["Wins"] = GetWins(r, leagueData),
                ["Losses"] = GetLosses(r, leagueData)
            }).ToList();
        return filteredTeams;
    }

    public string GetTeamName(JToken r, JObject leagueData)
    {
        return leagueData["teams"]?
            .FirstOrDefault(team => team["owners"]?
                .Any(fp =>
                    fp.ToString() == r["id"]?.ToString()) == true)?["name"]?.ToString() ?? string.Empty;
    }

    public decimal? GetPointsFor(JToken r, JObject leagueData)
    {
        return (decimal?)leagueData["teams"]?
            .FirstOrDefault(team => team["owners"]?
                .Any(fp =>
                    fp.ToString() == r["id"]?.ToString()) == true)?["record"]?["overall"]?["pointsFor"];
    }

    public decimal? GetPointsAgainst(JToken r, JObject leagueData)
    {
        return (decimal?)leagueData["teams"]?
            .FirstOrDefault(team => team["owners"]?
                .Any(fp =>
                    fp.ToString() == r["id"]?.ToString()) == true)?["record"]?["overall"]?["pointsAgainst"];
    }

    public int? GetWins(JToken r, JObject leagueData)
    {
        return (int?)leagueData["teams"]?
            .FirstOrDefault(team => team["owners"]?
                .Any(fp =>
                    fp.ToString() == r["id"]?.ToString()) == true)?["record"]?["overall"]?["wins"];
    }

    public int? GetLosses(JToken r, JObject leagueData)
    {
        return (int?)leagueData["teams"]?
            .FirstOrDefault(team => team["owners"]?
                .Any(fp =>
                    fp.ToString() == r["id"]?.ToString()) == true)?["record"]?["overall"]?["losses"];
    }

    public int? GetLeagueTeamId(JToken r, JObject leagueData)
    {
        return (int?)leagueData["teams"]?
            .FirstOrDefault(team => team["owners"]?
                .Any(fp =>
                    fp.ToString() == r["id"]?.ToString()) == true)?["id"];
    }
}