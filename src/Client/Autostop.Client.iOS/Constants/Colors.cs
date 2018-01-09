using Autostop.Client.Shared.UI;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace Autostop.Client.iOS.Constants
{
    public struct Colors
    {
        public static readonly UIColor PickupButtonColor = AutostopColors.PickupColorRgb.ToUIColor();
		public static readonly UIColor Accent = AutostopColors.PickupColorRgb.ToUIColor();
        public static readonly UIColor Gray50 = UIColor.FromRGB(250, 250, 250); 
        public static readonly UIColor Gray100 = UIColor.FromRGB(245, 245, 245);
        public static readonly UIColor Gray200 = UIColor.FromRGB(238, 238, 238);
        public static readonly UIColor Gray300 = UIColor.FromRGB(224, 224, 224);
        public static readonly UIColor Gray400 = UIColor.FromRGB(189, 189, 189);
        public static readonly UIColor Gray900 = UIColor.FromRGB(33, 33, 33);
    }
}