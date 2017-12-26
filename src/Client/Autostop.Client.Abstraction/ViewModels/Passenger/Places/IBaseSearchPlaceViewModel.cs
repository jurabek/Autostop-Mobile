using System.Collections.ObjectModel;
using Autostop.Client.Abstraction.Models;

namespace Autostop.Client.Abstraction.ViewModels.Passenger.Places
{
	public interface IBaseSearchPlaceViewModel : ISearchableViewModel
	{	
		ObservableCollection<IAutoCompleteResultModel> SearchResults { get; set; }
		IAutoCompleteResultModel SelectedSearchResult { get; set; }
	}
}