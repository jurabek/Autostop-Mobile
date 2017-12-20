using System;
using Autostop.Client.Core.Enums;
using Autostop.Client.iOS.Extensions;
using UIKit;

namespace Autostop.Client.iOS.Views.Passenger
{
    public partial class PickupAddressTextField : BaseAddressTextField
    {
        public PickupAddressTextField (IntPtr handle) : base (handle)
        {
        }

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