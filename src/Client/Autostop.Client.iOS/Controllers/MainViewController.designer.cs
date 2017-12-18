// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Autostop.Client.iOS.Controls;
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Autostop.Client.iOS.Controllers
{
    [Register ("MainViewController")]
    partial class MainViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView centerPinIcon { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Autostop.Client.iOS.Controls.UIDestinationAddressTextField destinationAddressTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Google.Maps.MapView gmsMapView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Autostop.Client.iOS.Controls.UIPickupAddressTextField pickupAddressTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton setPickupLocationButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (centerPinIcon != null) {
                centerPinIcon.Dispose ();
                centerPinIcon = null;
            }

            if (destinationAddressTextField != null) {
                destinationAddressTextField.Dispose ();
                destinationAddressTextField = null;
            }

            if (gmsMapView != null) {
                gmsMapView.Dispose ();
                gmsMapView = null;
            }

            if (pickupAddressTextField != null) {
                pickupAddressTextField.Dispose ();
                pickupAddressTextField = null;
            }

            if (setPickupLocationButton != null) {
                setPickupLocationButton.Dispose ();
                setPickupLocationButton = null;
            }
        }
    }
}