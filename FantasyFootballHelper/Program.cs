using System.Data;
using System.Net;
using Library.EspnApiInterface;
using Newtonsoft.Json;

namespace FantasyFootballHelper
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            int leagueId = 916800544;
            int seasonId = 2024;
            int playerId = 16800; // Davante Adams
            
            string url = $"https://lm-api-reads.fantasy.espn.com/apis/v3/games/ffl/seasons/{seasonId}/segments/0/leagues/{leagueId}?view=mRoster"; 
            string playerIdUrl = $"http://sports.core.api.espn.com/v2/sports/football/leagues/nfl/athletes/{playerId}?lang=en&region=us";
            string playerStatsUrl = $"http://sports.core.api.espn.com/v2/sports/football/leagues/nfl/seasons/2024/types/2/athletes/{playerId}/statistics/0";
            string defenseStatsUrl = $"https://sports.core.api.espn.com/v2/sports/football/leagues/nfl/seasons/2024/types/2/teams/{Library.EspnApiInterface.EspnApiInterfaceImp.TeamDictionary["Steelers"].TeamId}/statistics";
            
            var swid = "078B0B07-70ED-409D-9603-34EA37D12612";
            var espnS2 =
                "AEAQn%2FMeEWMlZoL1eTTPvgWjSY7Hs0gb4oDGDPSzom9h8q6urQ8JCb6BAxh34unJimg0WFKI5S%2FkHvwcwgv%2FaHmP" +
                "2n1G%2B6n4IOHDg2xSNZTRmaMvNP%2Fw77miYDkqXUFhkyYImbFKh5cYyxCLwKzWVP%2FcJMnTEnrodZ2T8gWcSHL" +
                "2Ti8Sp5K6hUPAHZS%2Bf%2Fyb86ZKVpdoaQJ9sz9Ln2bYwfCdoaWpZhHxs0bGjiK9Oa7fxEoKTofmaHxG%2FuQPC%2" +
                "BjYDbTKc3unGWd0TUO0pAYUic5KaxZHNMEfsnNzQUYycN0bMA%3D%3D";

            var cookieJar = new CookieContainer();
            cookieJar.Add(new Uri(url), new Cookie("espn_s2", espnS2));
            cookieJar.Add(new Uri(url), new Cookie("SWID", swid));
            
            var handler = new HttpClientHandler()
            {
                CookieContainer = cookieJar
            };

            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                var response = await client.GetAsync(defenseStatsUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Read and display the response
                    string content = await response.Content.ReadAsStringAsync();
                    File.WriteAllText(@"/Users/michaelbauer/Downloads/Defense_Stats.json", content);
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
        }
    }
}