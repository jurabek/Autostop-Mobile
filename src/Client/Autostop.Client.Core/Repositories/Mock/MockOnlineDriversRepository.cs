using System.Collections.ObjectModel;
using Autostop.Client.Abstraction.Repositores;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Core.Repositories.Mock
{
    class MockOnlineDriversRepository : IOnlineDriversRepository
    {
	    public ObservableCollection<DriverLocation> GetOnlineDriversLocation()
	    {
			return new ObservableCollection<DriverLocation>(MockData.AvailableDrivers);
	    }
    }
}
