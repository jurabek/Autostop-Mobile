using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Autostop.Client.Abstraction.Models;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Core.ViewModels.Passenger.LocationEditor.Base;

namespace Autostop.Client.Core.ViewModels.Passenger.LocationEditor
{
	public class PickupLocationEditorViewModel : BaseLocationEditorViewModel
	{
		private readonly IEmptyAutocompleteResultProvider _autocompleteResultProvider;

		public PickupLocationEditorViewModel(
			ISchedulerProvider schedulerProviderer,
			INavigationService navigationService,
			IPlacesProvider placesProvider,
			IGeocodingProvider geocodingProvider,
			IEmptyAutocompleteResultProvider autocompleteResultProvider) : base(schedulerProviderer, placesProvider, geocodingProvider, navigationService)
		{
			_autocompleteResultProvider = autocompleteResultProvider;
		}

        protected override ObservableCollection<IAutoCompleteResultModel> GetEmptyAutocompleteResult()
		{
			return new ObservableCollection<IAutoCompleteResultModel>
			{	
				_autocompleteResultProvider.GetHomeResultModel(),
				_autocompleteResultProvider.GetWorkResultModel()
			};
		}

	    public override string PlaceholderText => "Set pickup location";

	    public override Task Load()
	    {
	        SearchResults = GetEmptyAutocompleteResult();
	        return base.Load();
	    }
    }
}