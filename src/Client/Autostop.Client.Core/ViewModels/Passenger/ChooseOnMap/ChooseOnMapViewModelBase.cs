using System;
using System.Windows.Input;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Core.ViewModels.Passenger.ChooseOnMap
{
    public abstract class ChooseOnMapViewModelBase : BaseViewModel, ISearchableViewModel, IMapViewModel
    {
        private bool _isSearching;
        private Location _cameraTarget;
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
            get => _cameraTarget;
            set
            {
                _cameraTarget = value;
                RaisePropertyChanged();
            }
        }

        public virtual string PlaceholderText => "Search";

        public virtual ICommand GoBack { get; }

        public virtual ICommand Done { get; }

	    public IObservable<Location> MyLocationObservable { get; protected set; }

	    public IObservable<Location> CameraPositionObservable { get; set; }

	    public IObservable<bool> CameraStartMoving { get; set; }

	    public virtual IObservable<Address> SelectedAddress { get; }
	}
}
