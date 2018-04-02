using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Autostop.Client.Abstraction;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Android.Extensions;
using Autostop.Common.Shared.Models;
using GalaSoft.MvvmLight.Helpers;

namespace Autostop.Client.Android.Fragments
{
	public abstract class BaseChooseOnMapFragment<TViewModel> : Fragment, IScreenFor<TViewModel>, IOnMapReadyCallback
		where TViewModel : class, IMapViewModel, ISearchableViewModel
	{
		protected Button DoneButton { get; private set; }

		private EditText _addressEditText;
		private ProgressBar _selectedAddressLoading;
		private MapView _mapView;
		private ImageView _centeredMarkerIcon;
		private GoogleMap _googleMap;

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState)
		{
			base.OnViewCreated(view, savedInstanceState);
			_mapView = view.FindViewById<MapView>(Resource.Id.chooseOnMapFragmentMapView);
			_addressEditText = view.FindViewById<EditText>(Resource.Id.selecedAddressText);
			_selectedAddressLoading = view.FindViewById<ProgressBar>(Resource.Id.selectedAddressLoading);
			_centeredMarkerIcon = view.FindViewById<ImageView>(Resource.Id.chooseOnMapCenteredMarkerIcon);

			DoneButton = view.FindViewById<Button>(Resource.Id.chooseOnMapDoneButton);

			_mapView.OnCreate(savedInstanceState);
			_mapView.OnResume();
			_mapView.GetMapAsync(this);

			_centeredMarkerIcon.SetImageResource(GetPinImageResource());
			DoneButton.Text = GetDoneButtonTitle();
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			return inflater.Inflate(Resource.Layout.choose_on_map_fragment, container, false);
		}

		public TViewModel ViewModel { get; set; }

		protected abstract string GetDoneButtonTitle();

		protected abstract int GetPinImageResource();

		public void OnMapReady(GoogleMap googleMap)
		{
			_googleMap = googleMap;
			_googleMap.MyLocationEnabled = true;
			_googleMap.UiSettings.CompassEnabled = true;
			_googleMap.UiSettings.MyLocationButtonEnabled = true;

			var cameraPositionIdle = Observable
				.FromEventPattern<EventHandler, EventArgs>(
					e => _googleMap.CameraIdle += e,
					e => _googleMap.CameraIdle -= e);

			var cameraChanged = cameraPositionIdle
				.Select(_ => _googleMap.CameraPosition.Target)
				.Select(target => new Location(target.Latitude, target.Longitude));

			var cameraStartMoving = Observable
				.FromEventPattern<EventHandler<GoogleMap.CameraMoveStartedEventArgs>, GoogleMap.CameraMoveStartedEventArgs>(
					e => _googleMap.CameraMoveStarted += e,
					e => _googleMap.CameraMoveStarted -= e)
				.Select(e => true);

			ViewModel.CameraPositionChanged = cameraChanged;
			ViewModel.CameraStartMoving = cameraStartMoving;

			this.SetBinding(() => ViewModel.IsSearching)
				.WhenSourceChanges(() =>
				{
					_selectedAddressLoading.Visibility = ViewModel.IsSearching ? ViewStates.Visible : ViewStates.Gone;
				});

			this.SetBinding(
				() => ViewModel.SearchText,
				() => _addressEditText.Text);
		}
	}
}