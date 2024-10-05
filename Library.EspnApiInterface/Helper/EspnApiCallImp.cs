using System.Net;
using Microsoft.Extensions.Configuration;

namespace Library.EspnApiInterface.Helper;

public class EspnApiCallImp : IEspnApiCall
{
    private readonly IConfiguration _configuration;

    public EspnApiCallImp(
        IConfiguration configuration
    )
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }
    
    public CookieContainer SetUpEspnApiCookies()
    {
        var cookieJar = new CookieContainer();
        cookieJar.Add(new Uri(_configuration.GetSection("ApiEndpoints")["BaseAddress"] ?? string.Empty), new Cookie("espn_s2", _configuration["espnS2"]));
        cookieJar.Add(new Uri(_configuration.GetSection("ApiEndpoints")["BaseAddress"] ?? string.Empty), new Cookie("SWID", _configuration["swid"]));
        return cookieJar;
    }
    
    public dynamic SetUpFilter()
    {
        var filter = new
        {
            players = new
            {
                limit = 10000,
                sortPercOwned = new
                {
                    sortAsc = false,
                    sortPriority = 1
                },
            }
        };
        return filter;
    }
}