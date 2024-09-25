using Library.EspnApiInterface.DataModel;

namespace Library.EspnApiInterface;

public class EspnApiInterfaceImp
{
    private static IEnumerable<Team> _teams = [new Team { Abbreviation = "ARI", Name = "Cardinals", State = "Arizona", TeamId = 22},
                                               new Team { Abbreviation = "ATL", Name = "Falcons", State = "Georgia", TeamId = 1},
                                               new Team { Abbreviation = "BAL", Name = "Ravens", State = "Maryland", TeamId = 33},
                                               new Team { Abbreviation = "BUF", Name = "Bills", State = "New York", TeamId = 2},
                                               new Team { Abbreviation = "CAR", Name = "Panthers", State = "North Carolina", TeamId = 29},
                                               new Team { Abbreviation = "CHI", Name = "Bears", State = "Illinois", TeamId = 3},
                                               new Team { Abbreviation = "CIN", Name = "Bengals", State = "Ohio", TeamId = 4},
                                               new Team { Abbreviation = "CLE", Name = "Browns", State = "Ohio", TeamId = 5},
                                               new Team { Abbreviation = "DAL", Name = "Cowboys", State = "Texas", TeamId = 6},
                                               new Team { Abbreviation = "DEN", Name = "Broncos", State = "Colorado", TeamId = 7},
                                               new Team { Abbreviation = "DET", Name = "Lions", State = "Michigan", TeamId = 8},
                                               new Team { Abbreviation = "GB", Name = "Packers", State = "Wisconsin", TeamId = 9},
                                               new Team { Abbreviation = "HOU", Name = "Texans", State = "Texas", TeamId = 34},
                                               new Team { Abbreviation = "IND", Name = "Colts", State = "Indiana", TeamId = 10},
                                               new Team { Abbreviation = "JAX", Name = "Jaguars", State = "Florida", TeamId = 30},
                                               new Team { Abbreviation = "KC", Name = "Chiefs", State = "Missouri", TeamId = 12},
                                               new Team { Abbreviation = "LV", Name = "Raiders", State = "Nevada", TeamId = 13},
                                               new Team { Abbreviation = "LAC", Name = "Chargers", State = "California", TeamId = 24},
                                               new Team { Abbreviation = "LAR", Name = "Rams", State = "California", TeamId = 14},
                                               new Team { Abbreviation = "MIA", Name = "Dolphins", State = "Florida", TeamId = 15},
                                               new Team { Abbreviation = "MIN", Name = "Vikings", State = "Minnesota", TeamId = 26},
                                               new Team { Abbreviation = "NE", Name = "Patriots", State = "Massachusetts", TeamId = 17},
                                               new Team { Abbreviation = "NO", Name = "Saints", State = "Louisiana", TeamId = 18},
                                               new Team { Abbreviation = "NYG", Name = "Giants", State = "New Jersey", TeamId = 19},
                                               new Team { Abbreviation = "NYJ", Name = "Jets", State = "New Jersey", TeamId = 20},
                                               new Team { Abbreviation = "PHI", Name = "Eagles", State = "Pennsylvania", TeamId = 21},
                                               new Team { Abbreviation = "PIT", Name = "Steelers", State = "Pennsylvania", TeamId = 23},
                                               new Team { Abbreviation = "SF", Name = "49ers", State = "California", TeamId = 25},
                                               new Team { Abbreviation = "SEA", Name = "Seahawks", State = "Washington", TeamId = 26},
                                               new Team { Abbreviation = "TB", Name = "Buccaneers", State = "Florida", TeamId = 27},
                                               new Team { Abbreviation = "TEN", Name = "Titans", State = "Tennessee", TeamId = 10},
                                               new Team { Abbreviation = "WAS", Name = "Commanders", State = "Maryland", TeamId = 28}
    ];

    public static Dictionary<string, Team> TeamDictionary = _teams.ToDictionary(t => t.Name);
}