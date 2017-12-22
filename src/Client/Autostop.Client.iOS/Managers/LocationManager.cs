using System;
using System.Linq;
using System.Reactive.Linq;
using Autostop.Client.Abstraction.Managers;
using Autostop.Common.Shared.Models;
using CoreLocation;
using UIKit;

namespace Autostop.Client.iOS.Managers
{
    public class LocationManager : ILocationManager
    {
        private readonly CLLocationManager _localtionManager;

        public LocationManager()
        {
            _localtionManager = new CLLocationManager {PausesLocationUpdatesAutomatically = false};
            _localtionManager.RequestWhenInUseAuthorization();

            //if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            //    _localtionManager.RequestAlwaysAuthorization();

            //if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            //    _localtionManager.AllowsBackgroundLocationUpdates = true;

            LocationChanged = Observable
                .FromEventPattern<EventHandler<CLLocationsUpdatedEventArgs>, CLLocationsUpdatedEventArgs>
                (e => _localtionManager.LocationsUpdated += e, e => _localtionManager.LocationsUpdated -= e)
                .Select(x => x.EventArgs.Locations.LastOrDefault())
                .Where(location => location != null && location.Coordinate.IsValid())
                .Select(location => location.Coordinate)
                .Select(c => new Location(c.Latitude, c.Longitude))
                .Do(l => Location = l);

            HeadingChanged = Observable
                .FromEventPattern<EventHandler<CLHeadingUpdatedEventArgs>, CLHeadingUpdatedEventArgs>
                (e => _localtionManager.UpdatedHeading += e, e => _localtionManager.UpdatedHeading -= e)
                .Select(e => e.EventArgs.NewHeading.TrueHeading);
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
        public IObservable<double> HeadingChanged { get; }

        public Location Location { get; private set; }

        public void Dispose()
        {
            _localtionManager?.Dispose();
        }
    }
}