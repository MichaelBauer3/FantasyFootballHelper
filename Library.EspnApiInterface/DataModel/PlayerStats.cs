namespace Library.EspnApiInterface.DataModel;

public class PlayerStats
{
    public Player? Player { get; set; }
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