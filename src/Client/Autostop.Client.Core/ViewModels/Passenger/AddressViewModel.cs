using System;
using System.Collections.Generic;
using System.Text;

namespace Autostop.Client.Core.ViewModels.Passenger
{
	public class AddressViewModel : BaseViewModel
	{
		private string _formattedAddress;

		public string FormattedAddress
		{
			get => _formattedAddress;
			set => this.RaiseAndSetIfChanged(ref _formattedAddress, value);
		}

		public string PlaceId { get; set; }
	}
}
