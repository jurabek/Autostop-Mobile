using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Autostop.Client.Abstraction.ViewModels.Passenger.Places;

namespace Autostop.Client.Core.ViewModels.Passenger
{
    public abstract class BaseChooseOnMapViewModel : BaseViewModel, ISearchableViewModel
	{
		public bool IsSearching { get; set; }
		public string PlaceholderText { get; set; }
		public string SearchText { get; set; }
		public ICommand GoBack { get; protected set; }
	}
}
