using Cocona;
using Cocona.Builder;
using FantasyFootballHelper.Commands;
using FantasyFootballHelper.Commands.CommandHelpers.Generic;
using FantasyFootballHelper.Commands.CommandHelpers.SetUpApiHelper;
using Library.EspnApiInterface.Helper;
using Library.FantasyFootballDBInterface;
using Library.FantasyFootballDBInterface.FantasyFootballDBMySqlInterface;
using Library.FantasyFootballDBInterface.SqlReader;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            builder.Services.AddSingleton<IAvailableWaivers, AvailableWaiverImp>();
            builder.Services.AddLogging();
            
            var app = builder.Build();
            
            app.AddCommands<SetUpApiImp>();
            app.AddCommands<GetWaiverWirePlayersImp>();
            app.AddCommands<CommandRunner>();

            app.Run();
        }

        public static void SetUpTransients(CoconaAppBuilder builder)
        {
            builder.Services.AddTransient<ICoconaContextWrapper, CoconaContextWrapperImp>();
            builder.Services.AddTransient<ISetUpApiHelper, SetUpApiHelperImp>();
            builder.Services.AddTransient<IEspnApiCall, EspnApiCallImp>();
            builder.Services.AddTransient<ISetUpApi, SetUpApiImp>();
            builder.Services.AddTransient<IGetWaiverWirePlayers, GetWaiverWirePlayersImp>();
            builder.Services.AddTransient<ISetUpApiHelper, SetUpApiHelperImp>();
            builder.Services.AddTransient<IFantasyFootballDbMySqlInterface, FantasyFootballDbMySqlImp>();
            builder.Services.AddTransient<IFantasyFootballDbInterface, FantasyFootballDbInterfaceImp>();
            builder.Services.AddTransient<ISqlFileReader, SqlFileReaderImp>();
        }
    }
}