using System;
using System.Collections.Generic;
using System.Text;
using CoreAnimation;
using CoreGraphics;
using UIKit;

namespace Autostop.Client.iOS.Extensions
{
	public static class UIViewExtenstions
	{
		public static void RoundCorners(this UIView view, UIRectCorner corners, nfloat radius)
		{
			var path = UIBezierPath.FromRoundedRect(view.Bounds, corners, new CGSize(radius, radius));

			var mask = new CAShapeLayer
			{
				Path = path.CGPath
			};

			view.Layer.Mask = mask;
		}
	}
}
