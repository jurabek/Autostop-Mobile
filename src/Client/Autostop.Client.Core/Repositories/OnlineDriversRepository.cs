using System.Collections.ObjectModel;
using Autostop.Client.Abstraction.Repositores;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Core.Repositories
{
    public class OnlineDriversRepository : IOnlineDriversRepository
	{
		public ObservableCollection<DriverLocation> GetOnlineDriversLocation()
		{
			throw new System.NotImplementedException();
		}
	}
}
