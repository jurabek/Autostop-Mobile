using Autostop.Client.Core.ViewModels.Passenger;
using Autostop.Client.Core.ViewModels.Passenger.ChooseOnMap;
using Autostop.Client.iOS.Constants;
using Autostop.Client.iOS.Extensions;
using CoreGraphics;
using CoreLocation;
using Google.Maps;
using JetBrains.Annotations;
using UIKit;

namespace Autostop.Client.iOS.Views.Passenger
{
	public class ChooseDestinationOnMapViewController : BaseChooseOnMapViewController<ChooseDestinationOnMapViewModel>
	{
		public override async void ViewDidLoad()
		{
			base.ViewDidLoad();

		    this.BindCommand(DoneButton, ViewModel.Done);

		    var pickupLocation = ViewModel.CameraTarget;
            var unused = new Marker
		    {
		        Position = new CLLocationCoordinate2D(pickupLocation.Latitude, pickupLocation.Longitude),
		        IconView = new UIImageView(new CGRect(0, 0, 40, 40)) { Image = Icons.PickupPin },
		        Map = MapView
		    };
		    await ViewModel.Load();
        }

        protected override UIImage GetPinImage() => Icons.DestinationPin;

		protected override string GetDoneButtonTitle() => "SET DESTINATION";
	}
}
