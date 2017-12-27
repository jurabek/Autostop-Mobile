using System;
using System.Reactive.Linq;
using Autostop.Client.Core.ViewModels.Passenger;
using Autostop.Client.iOS.Constants;
using Autostop.Client.iOS.Extensions;
using CoreGraphics;
using CoreLocation;
using GalaSoft.MvvmLight.Helpers;
using Google.Maps;
using JetBrains.Annotations;
using UIKit;
using Location = Autostop.Common.Shared.Models.Location;

namespace Autostop.Client.iOS.Views.Passenger
{
	[UsedImplicitly]
	public class ChooseDestinationOnMapViewController : BaseChooseOnMapViewController<ChooseDestinationOnMapViewModel>
	{
		public override async void ViewDidLoad()
		{
			base.ViewDidLoad();

			this.BindCommand(DoneButton, ViewModel.Done);

            this.SetBinding(
		            () => MapView.Camera,
		            () => ViewModel.CameraTarget, BindingMode.TwoWay)
		        .ConvertTargetToSource(location =>
		            CameraPosition.FromCamera(location.Latitude, location.Longitude, 15));

		    ViewModel.CameraPositionObservable = Observable
		        .FromEventPattern<EventHandler<GMSCameraEventArgs>, GMSCameraEventArgs>(
		            e => MapView.CameraPositionIdle += e,
		            e => MapView.CameraPositionIdle -= e)
		        .Select(e => e.EventArgs.Position.Target)
		        .Select(c => new Location(c.Latitude, c.Longitude));

		    ViewModel.CameraStartMoving = Observable
		        .FromEventPattern<EventHandler<GMSWillMoveEventArgs>, GMSWillMoveEventArgs>(
		            e => MapView.WillMove += e,
		            e => MapView.WillMove -= e)
		        .Select(e => e.EventArgs.Gesture);

            await ViewModel.Load();

		    var pickupLocation = ViewModel.RideViewModel.PickupAddress.Location;
            var unused = new Marker
		    {
		        Position = new CLLocationCoordinate2D(pickupLocation.Latitude, pickupLocation.Longitude),
		        IconView = new UIImageView(new CGRect(0, 0, 40, 40)) { Image = Icons.PickupPin },
		        Map = MapView
		    };
        }

		protected override UIImage GetPinImage()
		{
			return Icons.DestinationPin;
		}

		protected override string GetDoneButtonTitle()
		{
			return "SET DESTINATION";
		}
	}
}
