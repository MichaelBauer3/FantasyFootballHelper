namespace Library.EspnApiInterface.DataModel;

public class Players
{
    public required int? PlayerId { get; set; }
    public required string? FirstName { get; set; }
    public required string? LastName { get; set; }
    public required string? Position { get; set; }
    public required string? TeamId { get; set; }
    
    public int? Targets { get; set; }
    public int? Catches { get; set; }
    public decimal? ReceivingYards { get; set; }
    public int? ReceivingTouchdowns { get; set; }
    
    public decimal? RushingYards { get; set; }
    public int? RushingTouchdowns { get; set; }
    
    public decimal? PassingYards { get; set; }
    public int? PassingTouchdowns { get; set; }
    public int? Interceptions { get; set; }
    
    public int? Turnovers { get; set; }
    public int? Sacks { get; set; }
    
}