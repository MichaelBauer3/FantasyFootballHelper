using Microsoft.Extensions.Configuration;

namespace FantasyFootballHelper.Commands.CommandHelpers.SetUpApiHelper;

public class SetUpApiHelperImp : ISetUpApiHelper
{
    private readonly IConfiguration _configuration;
    
    public SetUpApiHelperImp(
        IConfiguration configuration
        )
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }
    
    public IEnumerable<string> GetApiEndPoints(Dictionary<string, string> placeholders)
    {
        var endpoints = new List<string>();
        var apiEndPoints = _configuration.GetSection("ApiEndpoints").GetChildren();
        foreach (var apiEndPoint in apiEndPoints)
        {
            var value = apiEndPoint.Value;
            if (!string.IsNullOrEmpty(value))
            {
                foreach (var placeholder in placeholders)
                {
                    value = value.Replace(placeholder.Key, placeholder.Value);
                }
                endpoints.Add(value);
            }
        }
        return endpoints;
    }
}