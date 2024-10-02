using Microsoft.VisualBasic;

namespace FantasyFootballHelper.Commands.CommandHelpers.CommandRunnerHelper;

public class CommandRunnerHelperImp : ICommandRunnerHelper
{
    private const int ThreeDaysToGetThursday = 3;
    private const int DaysPerWeek = 7;
    
    public string CalculateCurrentNflSeasonWeek(string? year)
    {
        int.TryParse(year, out int season);
        DateTime firstMondayOfSeptember = new DateTime(season, 9, 1);
        int daysToAdd = ((int)DayOfWeek.Monday - (int)firstMondayOfSeptember.DayOfWeek + DaysPerWeek) % DaysPerWeek;
        
        DateTime nflWeekOneStart = firstMondayOfSeptember.AddDays(daysToAdd + ThreeDaysToGetThursday);

        var today = new DateTime(2024, 10, 2);//DateTime.Today;
        int daysSinceFirstNflGame = (today - nflWeekOneStart).Days;
        return Math.Max((daysSinceFirstNflGame/DaysPerWeek) + 1 , 1).ToString();
    }
}