using System;
using System.Collections.Generic;
using System.Text;
using Autostop.Client.Core.Enums;
using Autostop.Client.iOS.Extensions;
using UIKit;

namespace Autostop.Client.iOS.UI
{
	public class DestinationAddressTextField : BaseAddressTextField
	{
		protected override string LeftImageSource => "pickup_destination_dot.png";

		public override AddressMode Mode
		{
			get => base.Mode;
			set
			{
				base.Mode = value;
				switch (value)
				{
					case AddressMode.Pickup:
						Alpha = 0;
						break;
					case AddressMode.Destination:
						Alpha = 1;
						this.RoundCorners(UIRectCorner.BottomLeft | UIRectCorner.BottomRight, 5);
						break;
				}
			}
		}
	}
}
