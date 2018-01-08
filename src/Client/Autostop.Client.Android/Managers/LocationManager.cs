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
using ALocationProvider = Android.Locations.LocationProvider;

namespace Autostop.Client.Android.Managers
{
	[UsedImplicitly]
	[Preserve(AllMembers = true)]
	public class LocationManager : Java.Lang.Object, ILocationManager, ILocationListener
	{
		private readonly HashSet<string> activeProviders = new HashSet<string>();
		private readonly Subject<Location> _locationChanged = new Subject<Location>();
		private readonly ALocationManager _locationManager;

		private readonly string[] Providers;
		private readonly string[] IgnoredProviders;
		string activeProvider;

		public LocationManager()
		{
			_locationManager = (ALocationManager)Application.Context.GetSystemService(Context.LocationService);
			Providers = _locationManager.GetProviders(false).ToArray();
			IgnoredProviders = new[] { ALocationManager.PassiveProvider, "local_database" };
		}

		public IObservable<Location> LocationChanged => _locationChanged;

		public Location Location { get; private set; }

		List<string> listeningProviders { get; } = new List<string>();

		public void StartUpdatingLocation()
		{
			var providers = Providers;
			var looper = Looper.MyLooper() ?? Looper.MainLooper;
			listeningProviders.Clear();
			for (var i = 0; i < providers.Length; ++i)
			{
				var provider = providers[i];
				listeningProviders.Add(provider);
				_locationManager.RequestLocationUpdates(provider, 1000, 0, this, looper);
			}
		}

		public void StopUpdatingLocation()
		{
		}

		public void OnLocationChanged(global::Android.Locations.Location location)
		{
			activeProvider = location.Provider;
			var lastLocation = new Location(location.Latitude, location.Longitude);
			Location = lastLocation;
			_locationChanged.OnNext(lastLocation);
		}

		public void OnProviderDisabled(string provider)
		{
			if (provider == ALocationManager.PassiveProvider)
				return;

			lock (activeProviders)
			{
				activeProviders.Remove(provider);
			}
		}

		public void OnProviderEnabled(string provider)
		{
			if (provider == ALocationManager.PassiveProvider)
				return;

			lock (activeProviders)
				activeProviders.Add(provider);
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