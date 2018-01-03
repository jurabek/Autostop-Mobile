using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Autostop.Client.Abstraction.Managers;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Android.Managers
{
	class LocationManager : ILocationManager
	{
		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public IObservable<Location> LocationChanged { get; }
		public Location Location { get; }
		public void StartUpdatingLocation()
		{
			throw new NotImplementedException();
		}

		public void StopUpdatingLocation()
		{
			throw new NotImplementedException();
		}
	}
}