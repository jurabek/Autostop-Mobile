using System.Windows.Input;
using Autostop.Common.Shared.Enums;

namespace Autostop.Client.Core.ViewModels.Passenger
{
    public class MainViewModel : ViewModelBase
    {
	    public MainViewModel()
	    {
	    }

	    private bool _hasPickupLocation;
	    public bool HasPickupLocation
	    {
		    get => _hasPickupLocation;
		    set => RaiseAndSetIfChanged(ref _hasPickupLocation, value);
	    }

		public ICommand SetPickupLocation { get; }

		public ICommand SetDestination { get; }
	}
}
