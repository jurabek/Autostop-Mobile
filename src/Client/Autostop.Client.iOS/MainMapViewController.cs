using System;
using System.Linq;
using Autostop.Client.iOS.Extensions;
using CoreAnimation;
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
			CameraPosition camera = CameraPosition.FromCamera(37.797865, -122.402526, 14);
			MainMapView.Camera = camera;

			pickupLocationTextField.LeftViewMode = UITextFieldViewMode.Always;
			pickupLocationTextField.LeftView = new UIImageView(UIImage.FromFile("pickup_location_dot.png"));

			destinationTextField.LeftViewMode = UITextFieldViewMode.Always;
			destinationTextField.LeftView = new UIImageView(UIImage.FromFile("pickup_destination_dot.png"));

			pickupLocationTextField.RoundCorners(UIRectCorner.TopLeft | UIRectCorner.TopRight, 8);
			destinationTextField.RoundCorners(UIRectCorner.BottomLeft | UIRectCorner.BottomRight, 8);

			MainMapView.WillMove += MainMapView_WillMove;
		}

		private void MainMapView_WillMove(object sender, GMSWillMoveEventArgs e)
		{
		}
	}
}