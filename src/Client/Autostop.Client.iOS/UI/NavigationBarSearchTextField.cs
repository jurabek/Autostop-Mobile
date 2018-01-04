using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.iOS.Extensions;
using CoreGraphics;
using GalaSoft.MvvmLight.Helpers;
using UIKit;

namespace Autostop.Client.iOS.UI
{
	public sealed class NavigationBarSearchTextField : UITextField
	{
		private readonly UIActivityIndicatorView _loadingActivatyIndacator;
		private bool _isLoading;

		public NavigationBarSearchTextField(CGRect cgRect, ISearchableViewModel viewModel) : base(cgRect)
		{
			ViewModel = viewModel;
			_loadingActivatyIndacator = GetLocationsLoadActivityIndacator();
			var backButton = GetBackButton();

			LeftViewMode = UITextFieldViewMode.Always;
			RightViewMode = UITextFieldViewMode.Always;
			LeftView = backButton;

			BorderStyle = UITextBorderStyle.None;
			this.RoundCorners(UIRectCorner.AllCorners, 5);
			BackgroundColor = UIColor.White;

			ClearButtonMode = UITextFieldViewMode.WhileEditing;
			MinimumFontSize = 12;
			Font = UIFont.SystemFontOfSize(12, UIFontWeight.Regular);
			Placeholder = ViewModel.PlaceholderText;

			this.SetBinding(() => Text, () => ViewModel.SearchText, BindingMode.TwoWay);
			this.SetBinding(() => IsLoading, () => ViewModel.IsSearching, BindingMode.TwoWay);
			this.BindCommand(backButton, ViewModel.GoBack);
		}

		private ISearchableViewModel ViewModel { get; }

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
			return backButton;
		}

		private UIActivityIndicatorView GetLocationsLoadActivityIndacator()
		{
			var activityIndicator =
				new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray) { ContentMode = UIViewContentMode.Center };
			var size = activityIndicator.Frame.Size;
			activityIndicator.Frame = new CGRect(0, 0, size.Width + 15, size.Height);
			return activityIndicator;
		}
	}
}