using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Abstraction.ViewModels.Passenger;
using GalaSoft.MvvmLight.Command;

namespace Autostop.Client.Core.ViewModels.Passenger
{
    public class RideViewModel : BaseViewModel, IRideViewModel
	{
		private bool _isPickupAddressLoading;

		public RideViewModel()
		{
			SetPickupLocation = new RelayCommand(() =>
			{
				HasPickupLocation = true;
			});
		}

		public IAddressViewModel PickupAddress { get; } = new AddressViewModel();

	    public IAddressViewModel DestinationAddress { get; } = new AddressViewModel();

	    public ICommand SetPickupLocation { get; }

		public bool IsPickupAddressLoading
		{
			get => _isPickupAddressLoading;
			set => RaiseAndSetIfChanged(ref _isPickupAddressLoading, value);
		}
		
		private bool _hasPickupLocation;

		public bool HasPickupLocation
		{
			get => _hasPickupLocation;
			set => RaiseAndSetIfChanged(ref _hasPickupLocation, value);
		}
	}
}
