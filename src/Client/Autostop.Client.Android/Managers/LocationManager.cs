using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Autostop.Client.Abstraction.Managers;
using JetBrains.Annotations;
using Location = Autostop.Common.Shared.Models.Location;
using ALocationManager = Android.Locations.LocationManager;

namespace Autostop.Client.Android.Managers
{
    [UsedImplicitly]
	[Preserve(AllMembers = true)]
	public class LocationManager : Java.Lang.Object, ILocationManager, ILocationListener
	{
        private readonly HashSet<string> _activeProviders = new HashSet<string>();
		private readonly Subject<Location> _locationChanged = new Subject<Location>();
		private readonly ALocationManager _locationManager;

		private readonly string[] _providers;
		private readonly string[] _ignoredProviders;
		private string _activeProvider;

		public LocationManager()
		{
			_locationManager = (ALocationManager)Application.Context.GetSystemService(Context.LocationService);
			_providers = _locationManager.GetProviders(false).ToArray();
			_ignoredProviders = new[] { ALocationManager.PassiveProvider, "local_database" };
		}

		public IObservable<Location> LocationChanged => _locationChanged;
		public bool IsGeolocationEnabled => _providers.Any(p => !_ignoredProviders.Contains(p) &&
		                                                       _locationManager.IsProviderEnabled(p));
		public Location Location { get; private set; }

	    private List<string> ListeningProviders => new List<string>();
        public void StartUpdatingLocation()
		{
			if(!IsGeolocationEnabled)
				return;
			
			var providers = _providers;
			var looper = Looper.MyLooper() ?? Looper.MainLooper;
			ListeningProviders.Clear();
			foreach (var provider in providers)
			{
			    if (_ignoredProviders.Contains(provider))
			        continue;
			    
			    ListeningProviders.Add(provider);
			    _locationManager.RequestLocationUpdates(provider, 1000, 0, this, looper);
			}
		}

		public void StopUpdatingLocation()
		{
			var providers = ListeningProviders;

			for (var i = 0; i < providers.Count; i++)
			{
				try
				{
					_locationManager.RemoveUpdates(this);
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine("Unable to remove updates: " + ex);
				}
			}

		}

		public void OnLocationChanged(global::Android.Locations.Location location)
		{
			_activeProvider = location.Provider;
			var lastLocation = new Location(location.Latitude, location.Longitude);
			Location = lastLocation;
			_locationChanged.OnNext(lastLocation);
		}

		public void OnProviderDisabled(string provider)
		{
			if (provider == ALocationManager.PassiveProvider)
				return;

			lock (_activeProviders)
			{
				_activeProviders.Remove(provider);
			}
		}

		public void OnProviderEnabled(string provider)
		{
			if (provider == ALocationManager.PassiveProvider)
				return;

			lock (_activeProviders)
				_activeProviders.Add(provider);
		}

		public void OnStatusChanged(string provider, Availability status, Bundle extras)
		{
			switch (status)
			{
				case Availability.Available:
					OnProviderEnabled(provider);
					break;

				case Availability.OutOfService:
					OnProviderDisabled(provider);
					break;
			}
		}
	}
}