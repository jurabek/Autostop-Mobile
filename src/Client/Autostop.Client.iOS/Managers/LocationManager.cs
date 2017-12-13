using System;
using System.Linq;
using System.Reactive.Linq;
using Autostop.Client.Abstraction.Adapters;
using Autostop.Client.Abstraction.Managers;
using Autostop.Common.Shared.Models;
using CoreLocation;
using UIKit;

namespace Autostop.Client.iOS.Managers
{
	public class LocationManager : ILocationManager
	{
		private readonly CLLocationManager _localtionManager;

		public LocationManager(ILocationAdapter locationAdapter)
		{
			_localtionManager = new CLLocationManager { PausesLocationUpdatesAutomatically = false };

            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                _localtionManager.RequestAlwaysAuthorization();
            }

            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
			{
				_localtionManager.AllowsBackgroundLocationUpdates = true;
			}

			LocationChanged = Observable.FromEventPattern<EventHandler<CLLocationsUpdatedEventArgs>, CLLocationsUpdatedEventArgs>
				(e => _localtionManager.LocationsUpdated += e, e => _localtionManager.LocationsUpdated -= e)
				.Select(x => x.EventArgs.Locations.LastOrDefault())
				.Where(location => location != null && location.Coordinate.IsValid())
				.Select(location => location.Coordinate)
				.Select(c => locationAdapter.GetLocationFromCoordinate(c.Latitude, c.Longitude))
				.Do(l => CurrentLocation = l);
            
		}

        public void StartUpdatingLocation()
		{
			_localtionManager.StartUpdatingLocation();
		}

		public void StopUpdatingLocation()
		{
			_localtionManager.StopUpdatingLocation();
		}

		public IObservable<Location> LocationChanged { get; }

		public Location CurrentLocation { get; private set; }

		public void Dispose()
	    {
	        _localtionManager?.Dispose();
	    }
	}
}
