using System;
using System.Windows.Input;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Abstraction.ViewModels.Passenger.Places;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Core.ViewModels.Passenger
{
    public abstract class BaseChooseOnMapViewModel : BaseViewModel, ISearchableViewModel, IMapViewModel
    {
        private bool _isSearching;
        private Location _camerTarget;
        private string _searchText;

        public bool IsSearching
		{
		    get => _isSearching;
		    set => RaiseAndSetIfChanged(ref _isSearching, value);
		}

		public string SearchText
		{
		    get => _searchText;
		    set => RaiseAndSetIfChanged(ref _searchText, value);
		}

        public Location CameraTarget
        {
            get => _camerTarget;
            set
            {
                _camerTarget = value;
                RaisePropertyChanged();
            }
        }

        public virtual string PlaceholderText => "Search";

        public ICommand GoBack { get; protected set; }

        public ICommand Done { get; protected set; }

	    public IObservable<Location> MyLocationObservable { get; protected set; }

	    public IObservable<Location> CameraPositionObservable { get; set; }

	    public IObservable<bool> CameraStartMoving { get; set; }
	}
}
