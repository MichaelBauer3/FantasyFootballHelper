using System.Collections.Immutable;
using Library.EspnApiInterface.DataModel;

namespace FantasyFootballHelper.Commands;

public interface IGetFantasyPlayerStatistics
{
    Task<ImmutableDictionary<int, PlayerStats>> RunAsync(List<Player>? playersToInsert, IEnumerable<string> endpoints);
}