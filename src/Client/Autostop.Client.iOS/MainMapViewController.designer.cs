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
        UIKit.UIImageView CenterMarker { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Google.Maps.MapView MainMapView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SetPickupLocationButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CenterMarker != null) {
                CenterMarker.Dispose ();
                CenterMarker = null;
            }

            if (MainMapView != null) {
                MainMapView.Dispose ();
                MainMapView = null;
            }

            if (SetPickupLocationButton != null) {
                SetPickupLocationButton.Dispose ();
                SetPickupLocationButton = null;
            }
        }
    }
}