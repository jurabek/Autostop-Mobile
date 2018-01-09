using Autostop.Client.Shared.UI;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace Autostop.Client.iOS.UI
{
    public sealed class MyLocationButton : UIButton
    {
        public MyLocationButton()
        {
	        TranslatesAutoresizingMaskIntoConstraints = false;
			SetImage(UIImage.FromFile("location.png"), UIControlState.Normal);
            TintColor = AutostopColors.AsphaltRgb.ToUIColor();
            BackgroundColor = AutostopColors.White.ToUIColor();
            ImageEdgeInsets = new UIEdgeInsets(12, 12, 12, 12);
        }
    }
}