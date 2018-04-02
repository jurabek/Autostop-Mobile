using System;
using System.Collections.ObjectModel;
using Autostop.Client.Abstraction.Models;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Abstraction.ViewModels
{
	public interface IBaseLocationEditorViewModel : ISearchableViewModel
	{	
		ObservableCollection<IAutoCompleteResultModel> SearchResults { get; set; }

		IAutoCompleteResultModel SelectedSearchResult { get; set; }

		Address SelectedAddress { get; set; }
	}
}