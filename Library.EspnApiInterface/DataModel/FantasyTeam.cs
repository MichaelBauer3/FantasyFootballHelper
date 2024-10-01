namespace Library.EspnApiInterface.DataModel;

public class FantasyTeam
{
    public required string? TeamName { get; set; }
    public required string? TeamOwner { get; set; }
    public required decimal? PointsFor { get; set; }
    public required decimal? PointsAgainst { get; set; }
    public required int? Wins { get; set; }
    public required int? Losses { get; set; }
    public required IEnumerable<Player>? Roster { get; set; }
}