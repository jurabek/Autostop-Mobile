using System;
using System.Collections.Generic;
using System.Text;
using Autostop.Client.Core.Enums;
using Autostop.Client.iOS.Extensions;
using UIKit;

namespace Autostop.Client.iOS.UI
{
	class PickupAddressTextField : BaseAddressTextField
	{
		protected override string LeftImageSource => "pickup_location_dot.png";

		public override AddressMode Mode
		{
			get => base.Mode;
			set
			{
				base.Mode = value;

				switch (value)
				{
					case AddressMode.Pickup:
						this.RoundCorners(UIRectCorner.AllCorners, 5);
						break;
					case AddressMode.Destination:
						this.RoundCorners(UIRectCorner.TopLeft | UIRectCorner.TopRight, 5);
						break;
				}
			}
		}
	}
}
