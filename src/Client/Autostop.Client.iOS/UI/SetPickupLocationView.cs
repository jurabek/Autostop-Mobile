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
        public readonly UIButton RightArrowButton;


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
                //BackgroundColor = Colors.Accent,
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextColor = UIColor.White,
                Text = "2\nmin",
                Font = UIFont.SystemFontOfSize(10, UIFontWeight.Regular),
                TextAlignment = UITextAlignment.Center,
                LineBreakMode = UILineBreakMode.WordWrap,
                Lines = 0
            };

            //RightArrowButton = new UIButton()
            //{
            //    TranslatesAutoresizingMaskIntoConstraints = false,
            //    TintColor = UIColor.White,
            //    BackgroundColor = Colors.Accent,
            //    ContentEdgeInsets = new UIEdgeInsets(5, 5, 5, 5)
            //};
            //RightArrowButton.SetImage(UIImage.FromFile("right_arrow.png"), UIControlState.Normal);

            //SetPickupButton.Layer.CornerRadius = 20;
            //SetPickupButton.SetTitle("SET PICKUP LOCATION", UIControlState.Normal);


            //AddSubview(SetPickupButton);
            AddSubview(EstimatedTimeLabel);
            //AddSubview(RightArrowButton);
        }

        public override void UpdateConstraints()
        {
            base.UpdateConstraints();

            //NSLayoutConstraint.ActivateConstraints(new[]
            //{
            //    SetPickupButton.TrailingAnchor.ConstraintEqualTo(TrailingAnchor),
            //    SetPickupButton.LeadingAnchor.ConstraintEqualTo(LeadingAnchor),
            //    SetPickupButton.TopAnchor.ConstraintEqualTo(TopAnchor),
            //    SetPickupButton.BottomAnchor.ConstraintEqualTo(BottomAnchor)
            //});

            NSLayoutConstraint.ActivateConstraints(new[]
            {
                EstimatedTimeLabel.CenterXAnchor.ConstraintEqualTo(CenterXAnchor),
                EstimatedTimeLabel.CenterYAnchor.ConstraintEqualTo(CenterYAnchor),
                EstimatedTimeLabel.HeightAnchor.ConstraintEqualTo(30),
                EstimatedTimeLabel.WidthAnchor.ConstraintEqualTo(30),
            });

            //NSLayoutConstraint.ActivateConstraints(new[]
            //{
            //    RightArrowButton.TrailingAnchor.ConstraintEqualTo(TrailingAnchor, -5),
            //    RightArrowButton.CenterYAnchor.ConstraintEqualTo(CenterYAnchor),
            //    RightArrowButton.HeightAnchor.ConstraintEqualTo(30),
            //    RightArrowButton.WidthAnchor.ConstraintEqualTo(30),
            //});
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            //EstimatedTimeLabel.ToCircleView();
            //RightArrowButton.ToCircleView();
        }
    }
}