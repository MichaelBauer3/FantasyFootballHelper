using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Library.EspnApiInterface.Helper;

public interface IEspnApiCall
{
    CookieContainer SetUpEspnApiCookies();
}