using System;
using Autostop.Client.Core.Enums;
using Autostop.Client.iOS.Extensions;
using UIKit;

namespace Autostop.Client.iOS.Views.Passenger
{
	public partial class DestinationAddressTextField : BaseAddressTextField
	{
		public DestinationAddressTextField(IntPtr handle) : base(handle)
		{
		}

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
						Hidden = true;
						break;
					case AddressMode.Destination:
						Hidden = false;
						this.RoundCorners(UIRectCorner.BottomLeft | UIRectCorner.BottomRight, 5);
						break;
				}
			}
		}
	}
}