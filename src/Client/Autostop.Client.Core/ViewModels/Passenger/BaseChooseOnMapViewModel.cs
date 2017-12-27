﻿using System;
using System.Windows.Input;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Abstraction.ViewModels.Passenger.Places;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Core.ViewModels.Passenger
{
    public abstract class BaseChooseOnMapViewModel : BaseViewModel, ISearchableViewModel, IMapViewModel
    {
        private bool _isSearching;
		public bool IsSearching
		{
		    get => _isSearching;
		    set => RaiseAndSetIfChanged(ref _isSearching, value);
		}

        public virtual string PlaceholderText => "Search";

        private string _searchText;
		public string SearchText
		{
		    get => _searchText;
		    set => RaiseAndSetIfChanged(ref _searchText, value);
		}

		public ICommand GoBack { get; protected set; }

        public ICommand Done { get; protected set; }

	    public IObservable<Location> MyLocationObservable { get; protected set; }

	    public IObservable<Location> CameraPositionObservable { get; set; }

	    public IObservable<bool> CameraStartMoving { get; set; }

	    private Location _camerTarget;
	    public Location CameraTarget
	    {
	        get => _camerTarget;
	        set
	        {
	            _camerTarget = value;
	            RaisePropertyChanged();
	        }
	    }
	}
}
