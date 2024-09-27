using System.Net;

namespace Library.EspnApiInterface.Helper;

public class EspnApiCallImp : IEspnApiCall
{
    public CookieContainer CallEspnApi(string url)
    {
        var cookieJar = new CookieContainer();
        
        //cookieJar.Add(new Uri(url), new Cookie("espn_s2", Configuration["espnS2"]));
        //cookieJar.Add(new Uri(url), new Cookie("SWID", Configuration["swid"]));
        
        return cookieJar;
    }
}