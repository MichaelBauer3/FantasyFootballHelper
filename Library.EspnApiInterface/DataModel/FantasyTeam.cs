namespace Library.EspnApiInterface.DataModel;

public class FantasyTeam
{
    public required string? TeamName { get; set; }
    public required string? TeamOwnerId { get; set; }
    public required int? TeamId { get; set; }
    public required decimal? PointsFor { get; set; }
    public required decimal? PointsAgainst { get; set; }
    public required int? Wins { get; set; }
    public required int? Losses { get; set; }
}