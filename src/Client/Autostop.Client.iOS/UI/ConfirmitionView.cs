using System;
using System.Collections.Generic;
using System.Text;
using Autostop.Client.iOS.Constants;
using CoreAnimation;
using CoreGraphics;
using UIKit;

namespace Autostop.Client.iOS.UI
{
    public sealed class ConfirmitionView : UIView
    {
        private readonly UIButton _requestButton;
        private readonly UILabel _estimatedTimeLabel;
        private readonly UIStackView _bottomStackView;

        public ConfirmitionView()
        {
            BackgroundColor = UIColor.FromRGB(250, 250, 250);
            ClipsToBounds = true;

            _requestButton = new UIButton { BackgroundColor = Colors.PickupButtonColor };
            _requestButton.SetTitle("Request", UIControlState.Normal);

            _estimatedTimeLabel = new UILabel
            {
                Text = "Estimated arrival time 2 min",
                TextAlignment = UITextAlignment.Center,
                Font = UIFont.SystemFontOfSize(12, UIFontWeight.Regular)
            };

            _bottomStackView = new UIStackView(new UIView[] {_requestButton, _estimatedTimeLabel})
            {
                Axis = UILayoutConstraintAxis.Vertical,
                TranslatesAutoresizingMaskIntoConstraints = false,
                Spacing = 6,
                LayoutMarginsRelativeArrangement = true,
                LayoutMargins = new UIEdgeInsets(0, 15, 0, 15)
            };

            AddSubview(_bottomStackView);
        }

        public override void UpdateConstraints()
        {
            base.UpdateConstraints();
            NSLayoutConstraint.ActivateConstraints(new[]
            {
                _bottomStackView.BottomAnchor.ConstraintEqualTo(BottomAnchor, -10),
                _bottomStackView.LeadingAnchor.ConstraintEqualTo(LeadingAnchor),
                _bottomStackView.TrailingAnchor.ConstraintEqualTo(TrailingAnchor),
                _bottomStackView.HeightAnchor.ConstraintEqualTo(60)
            });
        }
    }
}
