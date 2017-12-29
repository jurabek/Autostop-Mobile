using Autostop.Client.iOS.Constants;
using Autostop.Client.iOS.Extensions;
using UIKit;

namespace Autostop.Client.iOS.UI
{
    public sealed class ConfirmitionView : UIView
    {
        private UIStackView _topStackView;
        private UIStackView _bottomStackView;
        private UIView _separator;
        private UIStackView _centerStackView;
        private UIImageView _paymentTypeImage;
        private UIButton _infoButton;

        public ConfirmitionView()
        {
            TranslatesAutoresizingMaskIntoConstraints = false;
            BackgroundColor = Colors.Gray50;
            ClipsToBounds = true;

            _infoButton = new UIButton
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TintColor = UIColor.Black,
                BackgroundColor = Colors.Gray100,
                ImageEdgeInsets = new UIEdgeInsets(5, 5, 5, 5)
            };
            _infoButton.SetImage(Icons.AutostopCar, UIControlState.Normal);

            var autostopLabel = new UILabel
            {
                Text = "Autostop",
                Font = UIFont.SystemFontOfSize(18, UIFontWeight.Regular),
                TextColor = Colors.Gray900
            };

            _topStackView = new UIStackView(new UIView[] {_infoButton, autostopLabel})
            {
                Axis = UILayoutConstraintAxis.Horizontal,
                Spacing = 15,
                TranslatesAutoresizingMaskIntoConstraints = false,
                LayoutMarginsRelativeArrangement = true,
                LayoutMargins = new UIEdgeInsets(0, 15, 0, 15)
            };

            CreateCenterStackView();

            CreateBottomStackView();

            _separator = new UIView
            {
                BackgroundColor = Colors.Gray200,
                TranslatesAutoresizingMaskIntoConstraints = false
            };

            AddSubview(_topStackView);
            AddSubview(_separator);
            AddSubview(_centerStackView);
            AddSubview(_bottomStackView);
        }

        private void CreateBottomStackView()
        {
            var requestButton = new UIButton { BackgroundColor = Colors.PickupButtonColor };
            requestButton.SetTitle("Request", UIControlState.Normal);
            requestButton.Layer.CornerRadius = 5;

            var estimatedTimeLabel = new UILabel
            {
                Text = "Estimated arrival time 2 min",
                TextAlignment = UITextAlignment.Center,
                Font = UIFont.SystemFontOfSize(12, UIFontWeight.Regular),
                TextColor = UIColor.LightGray
            };

            _bottomStackView = new UIStackView(new UIView[] { requestButton, estimatedTimeLabel })
            {
                Axis = UILayoutConstraintAxis.Vertical,
                TranslatesAutoresizingMaskIntoConstraints = false,
                Spacing = 6,
                LayoutMarginsRelativeArrangement = true,
                LayoutMargins = new UIEdgeInsets(0, 15, 0, 15)
            };
        }

        private void CreateCenterStackView()
        {
            _paymentTypeImage = new UIImageView(Icons.MasterCard) { TranslatesAutoresizingMaskIntoConstraints = false };
            var paymentTypeLabel = new UILabel
            {
                Text = "5937",
                Font = UIFont.SystemFontOfSize(12, UIFontWeight.Regular),
                TextColor = Colors.Gray900
            };

            _centerStackView = new UIStackView(new UIView[] { _paymentTypeImage, paymentTypeLabel })
            {
                Axis = UILayoutConstraintAxis.Horizontal,
                Spacing = 10,
                TranslatesAutoresizingMaskIntoConstraints = false,
                LayoutMarginsRelativeArrangement = true,
                LayoutMargins = new UIEdgeInsets(0, 30, 0, 15)
            };
        }

        public override void UpdateConstraints()
        {
            base.UpdateConstraints();

            _paymentTypeImage.WidthAnchor.ConstraintEqualTo(30).Active = true;
            _infoButton.WidthAnchor.ConstraintEqualTo(50).Active = true;
            _infoButton.HeightAnchor.ConstraintEqualTo(50).Active = true;

            NSLayoutConstraint.ActivateConstraints(new[]
            {
                _topStackView.TopAnchor.ConstraintEqualTo(TopAnchor, 10),
                _topStackView.LeadingAnchor.ConstraintEqualTo(LeadingAnchor),
                _topStackView.TrailingAnchor.ConstraintEqualTo(TrailingAnchor) 
            });

            NSLayoutConstraint.ActivateConstraints(new[]
            {
                _separator.LeadingAnchor.ConstraintEqualTo(LeadingAnchor),
                _separator.TrailingAnchor.ConstraintEqualTo(TrailingAnchor),
                _separator.HeightAnchor.ConstraintEqualTo(1),
                _separator.TopAnchor.ConstraintEqualTo(_topStackView.BottomAnchor, 10)
            });

            NSLayoutConstraint.ActivateConstraints(new[]
            {
                _centerStackView.LeadingAnchor.ConstraintEqualTo(LeadingAnchor),
                _centerStackView.TrailingAnchor.ConstraintEqualTo(TrailingAnchor),
                _centerStackView.HeightAnchor.ConstraintEqualTo(30),
                _centerStackView.CenterYAnchor.ConstraintEqualTo(CenterYAnchor)
            });

            NSLayoutConstraint.ActivateConstraints(new[]
            {
                _bottomStackView.BottomAnchor.ConstraintEqualTo(BottomAnchor, -10),
                _bottomStackView.LeadingAnchor.ConstraintEqualTo(LeadingAnchor),
                _bottomStackView.TrailingAnchor.ConstraintEqualTo(TrailingAnchor),
                _bottomStackView.HeightAnchor.ConstraintEqualTo(60)
            });
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            _infoButton.ToCircleButton();
        }
    }
}
