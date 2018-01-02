using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autostop.Client.iOS.Constants;
using Autostop.Client.iOS.Extensions;
using Foundation;
using UIKit;

namespace Autostop.Client.iOS.UI
{
    public sealed class SetPickupLocationView : UIView
    {
        public readonly UIButton SetPickupButton;
        public readonly UILabel EstimatedTimeLabel;

        public SetPickupLocationView()
        {
            TranslatesAutoresizingMaskIntoConstraints = false;
            ClipsToBounds = true;
           
            SetPickupButton = new UIButton
            {
                BackgroundColor = Colors.PickupButtonColor,
                TranslatesAutoresizingMaskIntoConstraints = false
            };

            EstimatedTimeLabel = new UILabel
            {
                BackgroundColor = Colors.Accent,
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextColor = UIColor.White,
                Text = "2\nmin",
                Font = UIFont.SystemFontOfSize(10, UIFontWeight.Light),
                TextAlignment = UITextAlignment.Center,
                LineBreakMode = UILineBreakMode.WordWrap,
                Lines = 0
            };

            SetPickupButton.Layer.CornerRadius = 20;
            SetPickupButton.SetTitle("SET PICKUP LOCATION", UIControlState.Normal);

            AddSubview(SetPickupButton);
            AddSubview(EstimatedTimeLabel);
        }

        public override void UpdateConstraints()
        {
            base.UpdateConstraints();

            NSLayoutConstraint.ActivateConstraints(new[]
            {
                SetPickupButton.TrailingAnchor.ConstraintEqualTo(TrailingAnchor),
                SetPickupButton.LeadingAnchor.ConstraintEqualTo(LeadingAnchor),
                SetPickupButton.TopAnchor.ConstraintEqualTo(TopAnchor),
                SetPickupButton.BottomAnchor.ConstraintEqualTo(BottomAnchor)
            });

            NSLayoutConstraint.ActivateConstraints(new []
            {
                EstimatedTimeLabel.LeadingAnchor.ConstraintEqualTo(LeadingAnchor, 10),
                EstimatedTimeLabel.CenterYAnchor.ConstraintEqualTo(CenterYAnchor),
                EstimatedTimeLabel.HeightAnchor.ConstraintEqualTo(30),
                EstimatedTimeLabel.WidthAnchor.ConstraintEqualTo(30),
            });
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            EstimatedTimeLabel.ToCircleView();
        }
    }
}