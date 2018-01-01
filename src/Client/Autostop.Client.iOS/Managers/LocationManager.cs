using System;
using System.Linq;
using System.Reactive.Linq;
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

			LocationChanged = Observable
				.FromEventPattern<EventHandler<CLLocationsUpdatedEventArgs>, CLLocationsUpdatedEventArgs>
				(e => _localtionManager.LocationsUpdated += e, e => _localtionManager.LocationsUpdated -= e)
				.Select(x => x.EventArgs.Locations.LastOrDefault())
				.Where(location => location != null && location.Coordinate.IsValid())
				.Select(location => location.Coordinate)
				.Select(c => c.ToLocation())
				.Do(l => Location = l);
		}

		public void StartUpdatingLocation()
		{
			_localtionManager.StartUpdatingLocation();
		}

		public void StopUpdatingLocation()
		{
			_localtionManager.StopUpdatingLocation();
		}

		public double Course => _localtionManager.Location.Course;

		public IObservable<Location> LocationChanged { get; }
		
		public Location Location { get; private set; }

		public void Dispose()
		{
			_localtionManager?.Dispose();
		}
	}
}