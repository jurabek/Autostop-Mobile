using System;
using System.Windows.Input;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Common.Shared.Models;
using JetBrains.Annotations;

namespace Autostop.Client.Core.ViewModels.Passenger.ChooseOnMap.Base
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

        [UsedImplicitly]
        public virtual ICommand GoBack { get; }

        [UsedImplicitly]
        public virtual ICommand Done { get; }

	    public IObservable<Location> MyLocationChanged { get; protected set; }

	    public IObservable<Location> CameraPositionChanged { get; set; }

	    public IObservable<bool> CameraStartMoving { get; set; }

	    [UsedImplicitly]
	    public virtual IObservable<Address> SelectedAddress { get; }
	}
}
