using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Autostop.Client.Abstraction.Managers;
using Autostop.Client.iOS.Extensions;
using Autostop.Common.Shared.Models;
using CoreLocation;
using JetBrains.Annotations;

namespace Autostop.Client.iOS.Managers
{
	[UsedImplicitly]
	public class LocationManager : ILocationManager
	{
		private readonly CLLocationManager _localtionManager;

		public LocationManager()
		{
			_localtionManager = new CLLocationManager { PausesLocationUpdatesAutomatically = false };
			_localtionManager.RequestWhenInUseAuthorization();
		    _localtionManager.AllowsBackgroundLocationUpdates = false;

			LocationChanged = Observable
				.FromEventPattern<EventHandler<CLLocationsUpdatedEventArgs>, CLLocationsUpdatedEventArgs>
				(e => _localtionManager.LocationsUpdated += e, e => _localtionManager.LocationsUpdated -= e)
				.Select(x => x.EventArgs.Locations.LastOrDefault())
				.Where(location => location != null && location.Coordinate.IsValid())
				.Select(location => location.Coordinate)
				.Select(c => c.ToLocation())
				.Do(l => LastKnownLocation = l);
		}

		public void RequestLocationUpdates()
		{
			_localtionManager.StartUpdatingLocation();
		}

		public void StopLocationUpdates()
		{
			_localtionManager.StopUpdatingLocation();
		}

	    public Task<Location> RequestSingleLocationUpdate()
	    {
			return Task.FromResult<Location>(new Location());
	    }
        
		public IObservable<Location> LocationChanged { get; }
		
		public Location LastKnownLocation { get; private set; }

		public void Dispose()
		{
			_localtionManager?.Dispose();
		}
	}
}