using System;
using System.Collections.Generic;
using System.Text;
using Autostop.Common.Shared.Models;
using CoreLocation;

namespace Autostop.Client.iOS.Extensions
{
	public static class LocationExtensions
	{
		public static Location ToLocation(this CLLocationCoordinate2D coordinate) =>
			new Location(coordinate.Latitude, coordinate.Longitude);

		public static CLLocationCoordinate2D ToCLLocationCoordinate2D(this Location location) =>
			new CLLocationCoordinate2D(location.Latitude, location.Longitude);
	}
}
