using Microsoft.Extensions.Configuration;

namespace FantasyFootballHelper.Commands.CommandHelpers.SetUpApiHelper;

public interface ISetUpApiHelper
{
    IEnumerable<string> GetApiEndPoints(Dictionary<string, string> placeholders);
}