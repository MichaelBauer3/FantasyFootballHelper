using Cocona;
using Cocona.Builder;
using FantasyFootballHelper.Commands;
using FantasyFootballHelper.Commands.CommandHelpers.CommandRunnerHelper;
using FantasyFootballHelper.Commands.CommandHelpers.Generic;
using FantasyFootballHelper.Commands.CommandHelpers.SetUpApiHelper;
using Library.EspnApiInterface.Helper;
using Library.EspnApiInterface.Helper.FantasyTeams;
using Library.EspnApiInterface.Helper.Players;
using Library.FantasyFootballDBInterface;
using Library.FantasyFootballDBInterface.FantasyFootballDBMySqlInterface;
using Library.FantasyFootballDBInterface.SqlReader;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Logging;

namespace FantasyFootballHelper
{
    internal class Program
    {
        private static void Main()
        {
            var builder = CoconaApp.CreateBuilder();
            
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets(typeof(Program).Assembly, optional: true)
                .Build();

            builder.Configuration.AddConfiguration(configuration);

            SetUpTransients(builder);
            builder.Services.AddSingleton<IConfiguration>(configuration);
            builder.Services.AddSingleton<IGetFantasyPlayersRosteredAndWaiver, GetFantasyPlayersRosteredAndWaiverImp>();
            builder.Services.AddSingleton<IFantasyTeamsFromLeague, FantasyTeamsFromLeagueImp>();
            builder.Services.AddLogging();
            builder.Logging.AddFilter("System.Net.Http", LogLevel.Warning);
            builder.Services.AddSingleton<HttpClientHandler>(serviceProvider =>
            {
                var espnApiCall = serviceProvider.GetRequiredService<IEspnApiCall>();
                var cookieJar = espnApiCall.SetUpEspnApiCookies();
                return new HttpClientHandler
                {
                    CookieContainer = cookieJar,
                };
            });
            builder.Services.AddHttpClient("SharedHttpClient")
                .ConfigureHttpClient(client =>
                {
                    client.BaseAddress = new Uri(configuration
                        .GetSection("ApiEndpoints")["BaseAddress"] ?? string.Empty);
                })
                .ConfigurePrimaryHttpMessageHandler(serviceProvider =>
                    serviceProvider.GetRequiredService<HttpClientHandler>()
                );
            
            var app = builder.Build();
            
            app.AddCommands<SetUpApiImp>();
            app.AddCommands<GetWaiverWirePlayersImp>();
            app.AddCommands<GetRosteredPlayersImp>();
            app.AddCommands<GetFantasyTeamsImp>();
            app.AddCommands<CommandRunner>();

            app.Run();
        }

        public static void SetUpTransients(CoconaAppBuilder builder)
        {
            builder.Services.AddTransient<ICoconaContextWrapper, CoconaContextWrapperImp>();
            builder.Services.AddTransient<ISetUpApiHelper, SetUpApiHelperImp>();
            builder.Services.AddTransient<IEspnApiCall, EspnApiCallImp>();
            builder.Services.AddTransient<ISetUpApiHelper, SetUpApiHelperImp>();
            builder.Services.AddTransient<IFantasyFootballDbMySqlInterface, FantasyFootballDbMySqlImp>();
            builder.Services.AddTransient<IFantasyFootballDbInterface, FantasyFootballDbInterfaceImp>();
            builder.Services.AddTransient<ISqlFileReader, SqlFileReaderImp>();
            
            // Commands - In the order they are used
            builder.Services.AddTransient<ISetUpApi, SetUpApiImp>();
            builder.Services.AddTransient<IGetWaiverWirePlayers, GetWaiverWirePlayersImp>();
            builder.Services.AddTransient<IGetRosteredPlayers, GetRosteredPlayersImp>();
            builder.Services.AddTransient<IGetFantasyTeams, GetFantasyTeamsImp>();
            builder.Services.AddTransient<ICommandRunnerHelper, CommandRunnerHelperImp>();
        }
    }
}