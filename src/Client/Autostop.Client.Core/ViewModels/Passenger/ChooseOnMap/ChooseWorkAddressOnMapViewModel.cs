using Autostop.Client.Abstraction.Managers;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Core.ViewModels.Passenger.ChooseOnMap.Base;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Core.ViewModels.Passenger.ChooseOnMap
{
    public class ChooseWorkAddressOnMapViewModel : ChooseAddressOnMapViewModelBase
    {
	    private readonly ISettingsProvider _settingsProvider;

	    public ChooseWorkAddressOnMapViewModel(
			ISettingsProvider settingsProvider,
            INavigationService navigationService,
            ILocationManager locationManager,
            IGeocodingProvider geocodingProvider) : base(navigationService, locationManager, geocodingProvider)
	    {
		    _settingsProvider = settingsProvider;
	    }

	    protected override void SetAddress(Address address)
	    {
			_settingsProvider.SetWorkAddress(address);
	    }
    }
}
