using System.Windows.Input;

namespace Autostop.Client.Abstraction.ViewModels.Passenger.Places
{
	public interface ISearchableViewModel
	{
		bool IsSearching { get; set; }
		string PlaceholderText { get; set; }
		string SearchText { get; set; }
		ICommand GoBack { get; }
	}
}
