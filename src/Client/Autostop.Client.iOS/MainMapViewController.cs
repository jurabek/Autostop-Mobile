using System;
using System.Linq;
using CoreGraphics;
using CoreLocation;
using Google.Maps;
using UIKit;


namespace Autostop.Client.iOS
{
	public partial class MainMapViewController : UIViewController
	{

		CLLocationManager localtionManager;

		public MainMapViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			localtionManager = new CLLocationManager();
			CameraPosition camera = CameraPosition.FromCamera(37.797865, -122.402526, 6);
			MainMapView.Camera = camera;
        }
	}
}