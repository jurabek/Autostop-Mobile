// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Autostop.Client.iOS.Views.Passenger
{
    [Register ("MainViewController")]
    partial class MainViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView centerPinIcon { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Autostop.Client.iOS.Views.Passenger.AutostopMapView gmsMapView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton myLocationButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton setPickupLocationButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (centerPinIcon != null) {
                centerPinIcon.Dispose ();
                centerPinIcon = null;
            }

            if (gmsMapView != null) {
                gmsMapView.Dispose ();
                gmsMapView = null;
            }

            if (myLocationButton != null) {
                myLocationButton.Dispose ();
                myLocationButton = null;
            }

            if (setPickupLocationButton != null) {
                setPickupLocationButton.Dispose ();
                setPickupLocationButton = null;
            }
        }
    }
}