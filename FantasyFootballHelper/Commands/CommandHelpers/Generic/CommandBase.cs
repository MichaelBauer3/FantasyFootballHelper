namespace FantasyFootballHelper.Commands.CommandHelpers.Generic;

public abstract class CommandBase
{
    protected readonly ICoconaContextWrapper CoconaContextWrapper;

    public CommandBase(
        ICoconaContextWrapper coconaContextWrapper
    )
    {
        CoconaContextWrapper = coconaContextWrapper ?? throw new ArgumentNullException(nameof(coconaContextWrapper)); 
    }
    
    protected CancellationToken CancellationToken => CoconaContextWrapper.CancellationToken;
}