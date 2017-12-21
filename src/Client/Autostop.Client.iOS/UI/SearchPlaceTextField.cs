using System;
using Autostop.Client.Core.ViewModels.Passenger;
using Autostop.Client.iOS.Extensions;
using CoreGraphics;
using GalaSoft.MvvmLight.Helpers;
using UIKit;

namespace Autostop.Client.iOS.UI
{
    public sealed class SearchPlaceTextField : UITextField
    {
        private readonly Action _backButtonAction;
        private readonly UIActivityIndicatorView _loadingActivatyIndacator;
        private bool _isLoading;

        public SearchPlaceTextField(CGRect cgRect, BaseSearchPlaceViewModel viewModel, Action backButtonAction) :
            base(cgRect)
        {
            ViewModel = viewModel;
            _backButtonAction = backButtonAction;
            _loadingActivatyIndacator = GetLocationsLoadActivityIndacator();

            LeftViewMode = UITextFieldViewMode.Always;
            RightViewMode = UITextFieldViewMode.Always;
            LeftView = GetBackButton();

            BorderStyle = UITextBorderStyle.None;
            this.RoundCorners(UIRectCorner.AllCorners, 5);
            BackgroundColor = UIColor.LightGray;

            ClearButtonMode = UITextFieldViewMode.WhileEditing;
            MinimumFontSize = 12;
            Font = UIFont.SystemFontOfSize(12, UIFontWeight.Regular);
            this.SetBinding(() => Text, () => ViewModel.SearchText, BindingMode.TwoWay);
            this.SetBinding(() => IsLoading, () => ViewModel.IsLoading, BindingMode.TwoWay);
        }

        public BaseSearchPlaceViewModel ViewModel { get; }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                if (_isLoading)
                {
                    _loadingActivatyIndacator.StartAnimating();
                    RightView = _loadingActivatyIndacator;
                }
                else
                {
                    _loadingActivatyIndacator.StopAnimating();
                    RightView = null;
                }
            }
        }

        private UIButton GetBackButton()
        {
            var image = UIImage.FromFile("back_icon.png");
            var backButton = new UIButton(UIButtonType.Custom)
            {
                Frame = new CGRect(0, 0, 45, 25),
                ImageEdgeInsets = new UIEdgeInsets(0, 10, 0, 10)
            };


            backButton.SetImage(image, UIControlState.Normal);

            backButton.TouchUpInside += (_, __) => _backButtonAction();

            return backButton;
        }


        private UIActivityIndicatorView GetLocationsLoadActivityIndacator()
        {
            var activityIndicator =
                new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray) {ContentMode = UIViewContentMode.Center};
            var size = activityIndicator.Frame.Size;
            activityIndicator.Frame = new CGRect(0, 0, size.Width + 15, size.Height);
            return activityIndicator;
        }
    }
}