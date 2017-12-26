using System;
using Autostop.Client.Abstraction.Services;
using Autostop.Common.Shared.Models;
using GalaSoft.MvvmLight.Command;

namespace Autostop.Client.Core.ViewModels.Passenger
{
    public sealed class ChooseDestinationOnMapViewModel : BaseChooseOnMapViewModel
    {
	    private readonly INavigationService _navigationService;

	    public ChooseDestinationOnMapViewModel(INavigationService navigationService)
	    {
		    _navigationService = navigationService;

		    GoBack = new RelayCommand(GoBackExecute);
	    }

	    private void GoBackExecute()
	    {
		    _navigationService.GoBack();
	    }
    }
}
