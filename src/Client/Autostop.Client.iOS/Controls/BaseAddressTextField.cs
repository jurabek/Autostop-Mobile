using System;
using Autostop.Client.Core.Enums;
using CoreGraphics;
using UIKit;

namespace Autostop.Client.iOS.Controls
{
	public abstract class BaseAddressTextField : UITextField
	{
		private readonly UIActivityIndicatorView _loadingActivatyIndacator;
		private readonly UIImageView _leftImageView;


		protected BaseAddressTextField(IntPtr handle) : base(handle)
		{
			_loadingActivatyIndacator = GetLocationsLoadActivityIndacator();
			_leftImageView = GetLocationsTextFieldLeftImageView();
			LeftViewMode = UITextFieldViewMode.Always;
			ShouldBeginEditing = _ => false;
		}

		private bool _loading;

		public virtual bool Loading
		{
			get => _loading;
			set
			{
				_loading = value;
				if (_loading)
				{
					_loadingActivatyIndacator.StartAnimating();
					LeftView = _loadingActivatyIndacator;
				}
				else
				{
					_loadingActivatyIndacator.StopAnimating();
					LeftView = _leftImageView;
				}
			}
		}

		private AddressMode _mode;

		public virtual AddressMode Mode
		{
			get => _mode;
			set => _mode = value;
		}


		private UIActivityIndicatorView GetLocationsLoadActivityIndacator()
		{
			var activityIndicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray) { ContentMode = UIViewContentMode.Center };
			var size = activityIndicator.Frame.Size;
			activityIndicator.Frame = new CGRect(0, 0, size.Width + 15, size.Height);
			return activityIndicator;
		}

		private UIImageView GetLocationsTextFieldLeftImageView()
		{
			var icon = new UIImageView(UIImage.FromFile(LeftImageSource));
			var size = icon.Image.Size;
			icon.ContentMode = UIViewContentMode.Center;
			icon.Frame = new CGRect(0, 0, size.Width + 10, size.Height);
			return icon;
		}

		protected abstract string LeftImageSource { get; }
	}
}