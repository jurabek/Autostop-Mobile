using System.Collections.ObjectModel;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Abstraction.Repositores
{
    public interface IOnlineDriversRepository
    {
        ObservableCollection<DriverLocation> GetOnlineDriversLocation();
    }
}