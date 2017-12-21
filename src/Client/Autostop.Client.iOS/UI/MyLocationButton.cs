using UIKit;

namespace Autostop.Client.iOS.UI
{
    public sealed class MyLocationButton : UIButton
    {
        public MyLocationButton()
        {
            SetImage(UIImage.FromFile("location.png"), UIControlState.Normal);
            TintColor = UIColor.Black;
            BackgroundColor = UIColor.White;
            ImageEdgeInsets = new UIEdgeInsets(10, 10, 10, 10);
        }
    }
}