using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Input;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Core.Extensions;
using Conditions;
using Google.Maps.Places;
using Google.Maps.Places.Autocomplete;

namespace Autostop.Client.Core.ViewModels.Passenger
{
    public abstract class BaseSearchPlaceViewModel : BaseViewModel
    {
        private bool _isLoading;
        private ObservableCollection<IAutoCompleteResultViewModel> _searchResults;
        private string _searchText;
        private AutocompleteResult _selectedSearchResult;

        protected BaseSearchPlaceViewModel(IPlacesProvider placesProvider)
        {
            placesProvider.Requires(nameof(placesProvider)).IsNotNull();

            this.ObservablePropertyChanged(() => SearchText)
                .Do(_ => IsLoading = true)
                .Throttle(TimeSpan.FromMilliseconds(500))
                .SubscribeOn(SynchronizationContext.Current)
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(async searchText =>
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(searchText))
                        {
                            var result = await placesProvider.GetAutoCompleteResponse(searchText);
                            SearchResults = result;
                        }
                        else
                        {
                            SearchResults = new ObservableCollection<IAutoCompleteResultViewModel>
                            {
                                new AutoCompleteResultViewModel
                                {
                                    PrimaryText = "Home",
                                    Icon = "home.png"
                                },
                                new AutoCompleteResultViewModel
                                {
                                    PrimaryText = "Work",
                                    Icon = "work.png"
                                },
                                new AutoCompleteResultViewModel
                                {
                                    PrimaryText = "Set location on map",
                                    Icon = "set_location.png"
                                }
                            };
                        }
                    }
                    catch (Exception)
                    {
                        SearchResults = null;
                    }
                    finally
                    {
                        IsLoading = false;
                    }
                });
        }


        public bool IsLoading
        {
            get => _isLoading;
            set => RaiseAndSetIfChanged(ref _isLoading, value);
        }

        public string SearchText
        {
            get => _searchText;
            set => RaiseAndSetIfChanged(ref _searchText, value);
        }

        public ObservableCollection<IAutoCompleteResultViewModel> SearchResults
        {
            get => _searchResults;
            set => RaiseAndSetIfChanged(ref _searchResults, value);
        }

        public AutocompleteResult SelectedSearchResult
        {
            get => _selectedSearchResult;
            set => RaiseAndSetIfChanged(ref _selectedSearchResult, value);
        }

        public ICommand ChooseOnMap { get; set; }

        public ICommand ChooseHomeAddress { get; set; }

        public ICommand ChooseWorkAddress { get; set; }
    }
}