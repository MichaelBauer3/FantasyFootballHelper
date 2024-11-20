using Library.EspnApiInterface.DataModel;

namespace Library.EspnApiInterface;

public class EspnApiInterfaceImp
{
    private static IEnumerable<Team> _teams = [new() { Abbreviation = "ARI", Name = "Cardinals", State = "Arizona", TeamId = 22},
                                               new() { Abbreviation = "ATL", Name = "Falcons", State = "Georgia", TeamId = 1},
                                               new() { Abbreviation = "BAL", Name = "Ravens", State = "Maryland", TeamId = 33},
                                               new() { Abbreviation = "BUF", Name = "Bills", State = "New York", TeamId = 2},
                                               new() { Abbreviation = "CAR", Name = "Panthers", State = "North Carolina", TeamId = 29},
                                               new() { Abbreviation = "CHI", Name = "Bears", State = "Illinois", TeamId = 3},
                                               new() { Abbreviation = "CIN", Name = "Bengals", State = "Ohio", TeamId = 4},
                                               new() { Abbreviation = "CLE", Name = "Browns", State = "Ohio", TeamId = 5},
                                               new() { Abbreviation = "DAL", Name = "Cowboys", State = "Texas", TeamId = 6},
                                               new() { Abbreviation = "DEN", Name = "Broncos", State = "Colorado", TeamId = 7},
                                               new() { Abbreviation = "DET", Name = "Lions", State = "Michigan", TeamId = 8},
                                               new() { Abbreviation = "GB", Name = "Packers", State = "Wisconsin", TeamId = 9},
                                               new() { Abbreviation = "HOU", Name = "Texans", State = "Texas", TeamId = 34},
                                               new() { Abbreviation = "IND", Name = "Colts", State = "Indiana", TeamId = 10},
                                               new() { Abbreviation = "JAX", Name = "Jaguars", State = "Florida", TeamId = 30},
                                               new() { Abbreviation = "KC", Name = "Chiefs", State = "Missouri", TeamId = 12},
                                               new() { Abbreviation = "LV", Name = "Raiders", State = "Nevada", TeamId = 13},
                                               new() { Abbreviation = "LAC", Name = "Chargers", State = "California", TeamId = 24},
                                               new() { Abbreviation = "LAR", Name = "Rams", State = "California", TeamId = 14},
                                               new() { Abbreviation = "MIA", Name = "Dolphins", State = "Florida", TeamId = 15},
                                               new() { Abbreviation = "MIN", Name = "Vikings", State = "Minnesota", TeamId = 26},
                                               new() { Abbreviation = "NE", Name = "Patriots", State = "Massachusetts", TeamId = 17},
                                               new() { Abbreviation = "NO", Name = "Saints", State = "Louisiana", TeamId = 18},
                                               new() { Abbreviation = "NYG", Name = "Giants", State = "New Jersey", TeamId = 19},
                                               new() { Abbreviation = "NYJ", Name = "Jets", State = "New Jersey", TeamId = 20},
                                               new() { Abbreviation = "PHI", Name = "Eagles", State = "Pennsylvania", TeamId = 21},
                                               new() { Abbreviation = "PIT", Name = "Steelers", State = "Pennsylvania", TeamId = 23},
                                               new() { Abbreviation = "SF", Name = "49ers", State = "California", TeamId = 25},
                                               new() { Abbreviation = "SEA", Name = "Seahawks", State = "Washington", TeamId = 26},
                                               new() { Abbreviation = "TB", Name = "Buccaneers", State = "Florida", TeamId = 27},
                                               new() { Abbreviation = "TEN", Name = "Titans", State = "Tennessee", TeamId = 10},
                                               new() { Abbreviation = "WAS", Name = "Commanders", State = "District of Columbia", TeamId = 28},
                                               new() { Abbreviation = "UNK", Name = string.Empty, State = "Unknown", TeamId = 32}
    ];

    public static Dictionary<string, Team> TeamDictionary = _teams.ToDictionary(t => t.Name);
    public static Dictionary<int, string> PositionsDictionary = new ()
    {
        {-1, "UNK"},
        {0, "QB"},
        {2, "RB"},
        {3, "WR"},
        {5, "TE"},
        {16, "D/ST"},
        {17, "K"}
    };
}