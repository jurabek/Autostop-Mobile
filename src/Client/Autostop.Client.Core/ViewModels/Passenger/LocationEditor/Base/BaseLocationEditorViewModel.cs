using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
using Autostop.Client.Abstraction.Models;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Core.Extensions;
using Autostop.Client.Core.Models;
using Autostop.Common.Shared.Models;
using GalaSoft.MvvmLight.Command;

namespace Autostop.Client.Core.ViewModels.Passenger.LocationEditor.Base
{
    public abstract class BaseLocationEditorViewModel : BaseViewModel, IBaseLocationEditorViewModel
    {
        private readonly INavigationService _navigationService;
        private ObservableCollection<IAutoCompleteResultModel> _searchResults;
        private IAutoCompleteResultModel _selectedSearchResult;
        private bool _isLoading;
        private string _searchText;
        private ICommand _goBack;

        protected BaseLocationEditorViewModel(
            ISchedulerProvider schedulerProvider,
            IPlacesProvider placesProvider,
            IGeocodingProvider geocodingProvider,
            INavigationService navigationService)
        {
            _navigationService = navigationService;

            this.Changed(() => SearchText)
				.Where(text => !string.IsNullOrEmpty(text))
                .Throttle(TimeSpan.FromMilliseconds(300), schedulerProvider.DefaultScheduler)
                .ObserveOn(schedulerProvider.SynchronizationContextScheduler)
                .Subscribe(async searchText =>
                {
                    IsSearching = true;
                    SearchResults = await placesProvider.GetAutoCompleteResponse(searchText);
                    IsSearching = false;
                });

	        this.Changed(() => SearchText)
		        .Where(string.IsNullOrEmpty)
		        .Subscribe(_ =>  SearchResults = GetEmptyAutocompleteResult());

			var selectedEmptyAutocompleteResultModelObservable = this.Changed(() => SelectedSearchResult)
                    .Where(result => result is EmptyAutocompleteResultModel)
                    .Cast<EmptyAutocompleteResultModel>();

            selectedEmptyAutocompleteResultModelObservable.Where(r => r.Address == null)
                .ObserveOn(schedulerProvider.SynchronizationContextScheduler)
                .Subscribe(selectedResult =>
                {
                    switch (selectedResult)
                    {
                        case HomeResultModel _:
                            _navigationService.NavigateToSearchView<HomeLocationEditorViewModel>(callBack: null);
                            break;
                        case WorkResultModel _:
                            _navigationService.NavigateToSearchView<WorkLocationEditorViewModel>(callBack: null);
                            break;
                    }
                });

            var selectedEmptyAddress = selectedEmptyAutocompleteResultModelObservable
                .Where(r => r.Address != null)
                .Select(r => r.Address);

            SelectedAddress = SelectedAutoCompleteResultModelObservable                
                .Select(x => Observable.FromAsync(() => geocodingProvider.ReverseGeocodingFromPlaceId(x.PlaceId)))
                .Concat()
                .Merge(selectedEmptyAddress);
        }

        protected IObservable<IAutoCompleteResultModel> SelectedAutoCompleteResultModelObservable => 
            this.Changed(() => SelectedSearchResult)
                .Where(r => r is AutoCompleteResultModel);

        public bool IsSearching
        {
            get => _isLoading;
            set => RaiseAndSetIfChanged(ref _isLoading, value);
        }

        public virtual string SearchText
        {
            get => _searchText;
            set => RaiseAndSetIfChanged(ref _searchText, value);
        }

        public ObservableCollection<IAutoCompleteResultModel> SearchResults
        {
            get => _searchResults;
            set => RaiseAndSetIfChanged(ref _searchResults, value);
        }

        public IAutoCompleteResultModel SelectedSearchResult
        {
            get => _selectedSearchResult;
            set => RaiseAndSetIfChanged(ref _selectedSearchResult, value);
        }

        public IObservable<Address> SelectedAddress { get; protected set; }

        public virtual ICommand GoBack => 
	        _goBack ?? (_goBack = new RelayCommand(() => _navigationService.GoBack()));

        protected abstract ObservableCollection<IAutoCompleteResultModel> GetEmptyAutocompleteResult();

	    public virtual string PlaceholderText => "Search";
	}
}