using System;
using System.Collections.Generic;
using System.Text;
using Autostop.Client.Abstraction.ViewModels.Passenger;

namespace Autostop.Client.Core.ViewModels.Passenger
{
	public class AddressViewModel : BaseViewModel, IAddressViewModel
    {
		private string _formattedAddress;

		public string FormattedAddress
		{
			get => _formattedAddress;
			set => this.RaiseAndSetIfChanged(ref _formattedAddress, value);
		}
	}
}
