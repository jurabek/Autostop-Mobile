using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Core.ViewModels.Passenger
{
    class OnlineDriversViewModel : BaseViewModel
    {
	    private ObservableCollection<DriverLocation> _onlineDrivers;

	    public ObservableCollection<DriverLocation> OnlineDrivers
	    {
		    get => _onlineDrivers;
		    private set
		    {
			    _onlineDrivers = value;
			    RaisePropertyChanged();
		    }
	    }
	}
}
