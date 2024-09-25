namespace Library.EspnApiInterface.DataModel;

public class Team
{
    public required string Name { get; set; }
    public required string Abbreviation { get; set; }
    public required string State { get; set; }
    public required int TeamId { get; set; }
}