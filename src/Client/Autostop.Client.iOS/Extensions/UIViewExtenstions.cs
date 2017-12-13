using System;
using CoreAnimation;
using CoreGraphics;
using UIKit;

namespace Autostop.Client.iOS.Extensions
{
	public static class UiViewExtenstions
	{
		public static void RoundCorners(this UIView view, UIRectCorner corners, nfloat radius)
		{
			var path = UIBezierPath.FromRoundedRect(view.Bounds, corners, new CGSize(radius, radius));
			view.Layer.Mask = new CAShapeLayer { Path = path.CGPath }; ;
		}
	}
}
