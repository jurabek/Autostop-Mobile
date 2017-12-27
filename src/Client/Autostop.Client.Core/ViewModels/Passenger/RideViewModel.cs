using System.Windows.Input;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Abstraction.ViewModels.Passenger;
using GalaSoft.MvvmLight.Command;
using JetBrains.Annotations;

namespace Autostop.Client.Core.ViewModels.Passenger
{
    [UsedImplicitly]
    public class RideViewModel : BaseViewModel, IRideViewModel
	{
		private bool _isPickupAddressLoading;
		private bool _hasPickupLocation;

		public RideViewModel()
		{
			SetPickupLocation = new RelayCommand(SetPickupLocationAction);
		}

		private void SetPickupLocationAction()
		{
			HasPickupLocation = true;
		}

		public IAddressViewModel PickupAddress { get; } = new AddressViewModel();

	    public IAddressViewModel DestinationAddress { get; } = new AddressViewModel();

	    public ICommand SetPickupLocation { get; }

		public bool IsPickupAddressLoading
		{
			get => _isPickupAddressLoading;
			set => RaiseAndSetIfChanged(ref _isPickupAddressLoading, value);
		}
		
		public bool HasPickupLocation
		{
			get => _hasPickupLocation;
			set => RaiseAndSetIfChanged(ref _hasPickupLocation, value);
		}
	}
}
