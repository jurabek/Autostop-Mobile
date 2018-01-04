using System.Windows.Input;

namespace Autostop.Client.Abstraction.ViewModels
{
	public interface ISearchableViewModel : IObservableViewModel
    {
		bool IsSearching { get; set; }
		string PlaceholderText { get; }
		string SearchText { get; set; }
		ICommand GoBack { get; }
	}
}
