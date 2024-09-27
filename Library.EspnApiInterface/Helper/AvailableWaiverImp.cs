using Library.EspnApiInterface.DataModel;

namespace Library.EspnApiInterface.Helper;

public class AvailableWaiverImp : IAvailableWaivers
{
    public async Task<IEnumerable<Players>> TopFiveRunningBacks()
    {

        await Task.Delay(5000);
        
        return new List<Players>();
    }
}