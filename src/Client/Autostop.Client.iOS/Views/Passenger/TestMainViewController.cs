using System;
using System.Collections.Generic;
using System.Text;
using Autostop.Client.iOS.Extensions;
using CoreGraphics;
using Google.Maps;
using UIKit;

namespace Autostop.Client.iOS.Views.Passenger
{
    public class TestMainViewController : UIViewController
    {
        private MapView _mapView;
        private UIButton _setPickupButton;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _mapView = new MapView(this.View.Frame);
            _setPickupButton = new UIButton(new CGRect(0, 0, 305, 35));
            _setPickupButton.RoundCorners(UIRectCorner.AllCorners, 20);
            _setPickupButton.BackgroundColor = UIColor.FromRGB(76, 217, 100);
            _setPickupButton.SetTitle("SET PICKUP LOCATION", UIControlState.Normal);

            View.AddSubview(_mapView);
            View.AddSubview(_setPickupButton);

            _mapView.TranslatesAutoresizingMaskIntoConstraints = false;
            _mapView.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;
            _mapView.CenterYAnchor.ConstraintEqualTo(View.CenterYAnchor).Active = true;
            _mapView.WidthAnchor.ConstraintEqualTo(View.WidthAnchor).Active = true;
            _mapView.HeightAnchor.ConstraintEqualTo(View.HeightAnchor).Active = true;

            NSLayoutConstraint.Create(_setPickupButton, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, _setPickupButton, NSLayoutAttribute.CenterX, 1, 0).Active = true;
            NSLayoutConstraint.Create(_setPickupButton, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, _setPickupButton, NSLayoutAttribute.CenterY, 1, 0).Active = true;
        }
    }
}
