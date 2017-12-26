using System;
using System.Collections.Generic;
using System.Text;
using Autostop.Client.iOS.Constants;
using UIKit;

namespace Autostop.Client.iOS.UI
{
	public sealed class RequestUIView : UIView
	{
		private readonly UIButton _requestButton = new UIButton
		{
			TranslatesAutoresizingMaskIntoConstraints = false,
			BackgroundColor = Colors.PickupButtonColor,
		};

		public RequestUIView()
		{	
			BackgroundColor = UIColor.FromRGB(245, 245, 245);
			AddSubview(_requestButton);
			_requestButton.SetTitle("Request", UIControlState.Normal);
			ClipsToBounds = true;
		}

		public override void UpdateConstraints()
		{
			base.UpdateConstraints();
			NSLayoutConstraint.ActivateConstraints(new[]
			{
				_requestButton.BottomAnchor.ConstraintEqualTo(BottomAnchor, -10),
				_requestButton.LeadingAnchor.ConstraintEqualTo(LeadingAnchor, 10),
				_requestButton.TrailingAnchor.ConstraintEqualTo(TrailingAnchor, -10),
				_requestButton.HeightAnchor.ConstraintEqualTo(40)
			});
		}
	}
}
