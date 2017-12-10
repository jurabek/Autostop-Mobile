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

        public MainMapViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            localtionManager = new CLLocationManager();
            CameraPosition camera = CameraPosition.FromCamera(latitude: 37.797865,
                longitude: -122.402526,
                zoom: 6);
            MainMapView.Camera = camera;
            MainMapView.BringSubviewToFront(CenterMarker);
            MainMapView.BringSubviewToFront(SetPickupLocationButton);
            NavigationController.NavigationBar.BackgroundColor = UIColor.Clear;
            NavigationController.NavigationBar.Translucent = true;
            AutomaticallyAdjustsScrollViewInsets = false;

        }
    }
}