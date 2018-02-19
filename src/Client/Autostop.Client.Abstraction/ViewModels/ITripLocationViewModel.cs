using System;
using System.Windows.Input;
using Autostop.Client.Abstraction.Models;

namespace Autostop.Client.Abstraction.ViewModels
{
	public interface ITripLocationViewModel : IObservableViewModel
	{
		IObservable<bool> PickupLocationChanged { get; }
		IAddressModel PickupAddress { get; }
		IAddressModel DestinationAddress { get; }
		ICommand NavigateToPickupSearch { get; }
		ICommand NavigateToWhereTo { get; }
		bool CanRequest { get; set; }
	}
}