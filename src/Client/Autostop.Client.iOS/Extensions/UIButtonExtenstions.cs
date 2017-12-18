using System;
using UIKit;

namespace Autostop.Client.iOS.Extensions
{
    public static class UIButtonExtenstions
    {
	    public static void ToCircleButton(this UIButton button)
	    {
		    button.Layer.CornerRadius = (nfloat) (0.5 * button.Bounds.Size.Width);
		    button.ClipsToBounds = true;
	    }
    }
}
