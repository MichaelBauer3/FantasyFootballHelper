namespace Library.EspnApiInterface.DataModel;

public class Player
{
    public required string? Name { get; set; }
    public required int? PlayerId { get; set; }
    public required int? ProTeamId { get; set; }
    public required int? OnTeamId { get; set; }
    public required string? Position { get; set; }
}