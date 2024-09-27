using Cocona;
using FantasyFootballHelper.Commands.CommandHelpers.Generic;
using Library.EspnApiInterface.Helper;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace FantasyFootballHelper.Commands;

public class GetDataAndSendToMySql : CommandBase
{
    private readonly IEspnApiCall _espnApiCall;
    private readonly ILogger<GetDataAndSendToMySql> _logger;

    public GetDataAndSendToMySql(
        IEspnApiCall espnApiCall,
        ILogger<GetDataAndSendToMySql> logger,
        ICoconaContextWrapper coconaContextWrapper) : base(coconaContextWrapper)
    {
        _espnApiCall = espnApiCall ?? throw new ArgumentNullException(nameof(espnApiCall));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [Command(
        "store-data-in-database",
        Description = "Stores data in database"
    )]
    public async Task RunAsync()
    {
        _logger.LogInformation("In");
        await Task.CompletedTask; 
    }
}