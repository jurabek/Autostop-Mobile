using System;
using System.Linq;
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

			RoundCorners(pickupLocationTextField, UIRectCorner.TopLeft | UIRectCorner.TopRight, 8);
			RoundCorners(destinationTextField, UIRectCorner.BottomLeft | UIRectCorner.BottomRight, 8);
		}

		void RoundCorners(UIView view, UIRectCorner corners, nfloat radius)
		{
			var path = UIBezierPath.FromRoundedRect(view.Bounds, corners, new CGSize(radius, radius));

			var mask = new CAShapeLayer
			{
				Path = path.CGPath
			};

			view.Layer.Mask = mask;
		}
	}
}