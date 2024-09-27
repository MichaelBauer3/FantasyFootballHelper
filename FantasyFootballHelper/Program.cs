using Cocona;
using FantasyFootballHelper.Commands;
using FantasyFootballHelper.Commands.CommandHelpers.Generic;
using Library.EspnApiInterface.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            builder.Services.AddTransient<ICoconaContextWrapper, CoconaContextWrapperImp>();
            builder.Services.AddSingleton<IEspnApiCall, EspnApiCallImp>();
            builder.Services.AddLogging();
            
            var app = builder.Build();
            
            app.AddCommands<GetDataAndSendToMySql>();

            app.Run();
        }
    }
}