using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Abstraction.ViewModels.Passenger.Places;

namespace Autostop.Client.Android.Services
{
	class NavigationService : INavigationService
	{
		public void NavigateTo(Type viewModelType)
		{
			throw new NotImplementedException();
		}

		public void NavigateTo(Type viewModelType, Action<object> configure)
		{
			throw new NotImplementedException();
		}

		public void NavigateTo<TViewModel>()
		{
			throw new NotImplementedException();
		}

		public void NavigateTo<TViewModel>(TViewModel viewModel)
		{
			throw new NotImplementedException();
		}

		public void NavigateToModal<TViewModel>()
		{
			throw new NotImplementedException();
		}

		public void NavigateTo<TViewModel>(Action<object, TViewModel> configure)
		{
			throw new NotImplementedException();
		}

		public void NavigateToSearchView<TViewModel>(Action<TViewModel> callBack) where TViewModel : ISearchableViewModel
		{
			throw new NotImplementedException();
		}

		public void NavigateToSearchView<TViewModel>(TViewModel viewModel) where TViewModel : ISearchableViewModel
		{
			throw new NotImplementedException();
		}

		public void GoBack()
		{
			throw new NotImplementedException();
		}

		public void NavigaeToRoot()
		{
			throw new NotImplementedException();
		}
	}
}