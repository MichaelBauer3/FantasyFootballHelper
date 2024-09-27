namespace FantasyFootballHelper.Commands.CommandHelpers.Generic;

public interface ICoconaContextWrapper
{
    CancellationToken  CancellationToken { get; }
}