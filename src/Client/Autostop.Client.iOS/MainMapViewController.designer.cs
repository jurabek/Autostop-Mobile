// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Autostop.Client.iOS
{
    [Register ("MainMapViewController")]
    partial class MainMapViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField destinationTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Google.Maps.MapView MainMapView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField pickupLocationTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SetPickupLocationButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (destinationTextField != null) {
                destinationTextField.Dispose ();
                destinationTextField = null;
            }

            if (MainMapView != null) {
                MainMapView.Dispose ();
                MainMapView = null;
            }

            if (pickupLocationTextField != null) {
                pickupLocationTextField.Dispose ();
                pickupLocationTextField = null;
            }

            if (SetPickupLocationButton != null) {
                SetPickupLocationButton.Dispose ();
                SetPickupLocationButton = null;
            }
        }
    }
}