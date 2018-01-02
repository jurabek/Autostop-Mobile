using System.Collections.ObjectModel;
using Autostop.Client.iOS.Constants;
using Autostop.Common.Shared.Models;
using CoreGraphics;
using CoreLocation;
using Google.Maps;
using UIKit;

namespace Autostop.Client.iOS.UI
{
	public sealed class MapView : Google.Maps.MapView
	{
		public MapView()
		{
			TranslatesAutoresizingMaskIntoConstraints = false;
			MyLocationEnabled = true;
		}

		private ObservableCollection<DriverLocation> _onlineDrivers;
		public ObservableCollection<DriverLocation> OnlineDrivers
		{
			get => _onlineDrivers;
			set
			{
				_onlineDrivers = value;

				foreach (var driverLocation in _onlineDrivers)
				{
					var unused = new Marker
					{
						Position = new CLLocationCoordinate2D(driverLocation.CurrentLocation.Latitude, driverLocation.CurrentLocation.Longitude),
						IconView = new UIImageView(new CGRect(0, 0, 20, 40)) { Image = Icons.Car },
						Map = this,
						Flat = true,
						GroundAnchor = new CGPoint(0.5, 0.5),
						Rotation = driverLocation.Bearing
					};
				}
			}
		}
	}
}