using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Autofac;
using Autostop.Client.Core;
using Autostop.Client.Core.Enums;
using Autostop.Client.Core.ViewModels.Passenger;
using Autostop.Common.Shared.Models;
using GalaSoft.MvvmLight.Helpers;
using Google.Maps;
using JetBrains.Annotations;
using UIKit;

namespace Autostop.Client.iOS.Controllers
{
    public partial class MainViewController : UIViewController
    {
        private readonly IContainer _container = BootstrapperBase.Container;
        [UsedImplicitly] private List<Binding> _bindings;
        [UsedImplicitly] private List<IDisposable> _subscribers;

		[UsedImplicitly] public MainViewModel ViewModel { get; set; }

        public MainViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			gmsMapView.MyLocationEnabled = true;

			ViewModel = _container.Resolve<MainViewModel>();
	        ViewModel.CameraLocationObservable = Observable
				.FromEventPattern<EventHandler<GMSCameraEventArgs>, GMSCameraEventArgs>(
			        e => gmsMapView.CameraPositionIdle += e,
			        e => gmsMapView.CameraPositionIdle -= e)
		        .Select(e => e.EventArgs.Position.Target)
		        .Select(c => new Location(c.Latitude, c.Longitude));

			_bindings = new List<Binding>
            {
                this.SetBinding(
                    () => pickupAddressTextField.Text,
                    () => ViewModel.PickupAddress.FormattedAddress, BindingMode.TwoWay),

                this.SetBinding(
                    () => destinationAddressTextField.Text,
                    () => ViewModel.DestinationAddress.FormattedAddress, BindingMode.TwoWay),
				
				this.SetBinding( 
					() => pickupAddressTextField.Mode,
					() => ViewModel.AddressMode, BindingMode.TwoWay),

	            this.SetBinding(
		            () => destinationAddressTextField.Mode,
		            () => ViewModel.AddressMode, BindingMode.TwoWay),

				this.SetBinding(
					() => pickupAddressTextField.Loading, 
					() => ViewModel.IsPickupAddressLoading, BindingMode.TwoWay),

				this.SetBinding(
					() => destinationAddressTextField.Loading, 
					() => ViewModel.IsDestinationAddressLoading, BindingMode.TwoWay)
            };

            _subscribers = new List<IDisposable>
            {
                ViewModel.CurrentLocationObservable.Subscribe(l =>
                    {
                        var camera = CameraPosition.FromCamera(l.Latitude, l.Longitude, 17);
                        gmsMapView.Camera = camera;
                    })
            };

			ViewModel.CameraLocationObservable
				.Subscribe(async location =>
				{
					Console.WriteLine(location);
					//await ViewModel.CameraLocationChanged(location);
				});

	        ViewModel.AddressMode = AddressMode.Pickup;

			setPickupLocationButton.SetCommand("TouchUpInside", ViewModel.SetPickupLocation);
        }
	}
}