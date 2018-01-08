using Android.App;
using Android.Gms.Common;
using Android.Gms.Maps;
using Android.OS;
using Android.Util;
using Android.Views;
using Autostop.Client.Abstraction;
using Autostop.Client.Core.ViewModels.Passenger;
using JetBrains.Annotations;
using System;
using System.Reactive.Linq;
using Android.Widget;
using GalaSoft.MvvmLight.Helpers;
using LatLng = Android.Gms.Maps.Model.LatLng;
using Location = Autostop.Common.Shared.Models.Location;
using MapView = Android.Gms.Maps.MapView;

namespace Autostop.Client.Android.Views
{
	[UsedImplicitly]
	public class MainFragment : Fragment, IScreenFor<MainViewModel>, IOnMapReadyCallback
	{
		private MapView _mapView;
		private EditText _pickupAddressEditText;
		private EditText _destinationAddressEditText;
		private GoogleMap _googleMap;

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			CheckGooglePlayServicesIsInstalled();
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			return inflater.Inflate(Resource.Layout.main_fragment, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState)
		{
			base.OnViewCreated(view, savedInstanceState);
			_pickupAddressEditText = view.FindViewById<EditText>(Resource.Id.pickupLocationAddressEditText);
			_destinationAddressEditText = view.FindViewById<EditText>(Resource.Id.destinationAddressEditText);

			_mapView = view.FindViewById<MapView>(Resource.Id.mapView);
			_mapView.OnCreate(savedInstanceState);
			_mapView.OnResume();
			_mapView.GetMapAsync(this);

			_pickupAddressEditText.SetCommand("Click", ViewModel.NavigateToPickupSearch);
			_destinationAddressEditText.SetCommand("Click", ViewModel.NavigateToDestinationSearch);

			this.SetBinding(() => ViewModel.CameraTarget, BindingMode.TwoWay)
				.WhenSourceChanges(() =>
				{
					var camera = CameraUpdateFactory.NewLatLngZoom(new LatLng(ViewModel.CameraTarget.Latitude, ViewModel.CameraTarget.Longitude), 17);
					_googleMap?.MoveCamera(camera);
				});

			this.SetBinding(
				() => _pickupAddressEditText.Text,
				() => ViewModel.RideViewModel.PickupAddress.FormattedAddress, BindingMode.TwoWay);
		}

		private void CheckGooglePlayServicesIsInstalled()
		{
			int queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(Activity);
			if (queryResult == ConnectionResult.Success)
			{
				Log.Info(Tag, "Google Play Services is installed on this device.");
			}
		}

		public MainViewModel ViewModel { get; set; }

		public async void OnMapReady(GoogleMap googleMap)
		{
			_googleMap = googleMap;
			_googleMap.MyLocationEnabled = true;

			var cameraPositionIdle = Observable
				.FromEventPattern<EventHandler<GoogleMap.CameraChangeEventArgs>, GoogleMap.CameraChangeEventArgs>(
					e => _googleMap.CameraChange += e,
					e => _googleMap.CameraChange -= e);

			ViewModel.CameraPositionObservable = cameraPositionIdle
				.Select(e => new Location(e.EventArgs.Position.Target.Latitude, e.EventArgs.Position.Target.Longitude));

			//ViewModel.VisibleRegionChanged = cameraPositionIdle
			//	.Select(_ => new CoordinateBounds(_mapView.Projection.VisibleRegion))
			//	.Select(bounds => _visibleRegionProvider.GetVisibleRegion(bounds.NorthEast.ToLocation(), bounds.SouthWest.ToLocation()));

			ViewModel.CameraStartMoving = Observable
				.FromEventPattern<EventHandler<GoogleMap.CameraMoveStartedEventArgs>, GoogleMap.CameraMoveStartedEventArgs>(
					e => _googleMap.CameraMoveStarted += e,
					e => _googleMap.CameraMoveStarted -= e)
				.Select(e => true);

			await ViewModel.Load();
		}

		public override void OnResume()
		{
			base.OnResume();
			_mapView.OnResume();
		}

		public override void OnPause()
		{
			base.OnPause();
			_mapView.OnPause();
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
			_mapView.OnDestroy();
		}

		public override void OnLowMemory()
		{
			base.OnLowMemory();
			_mapView.OnLowMemory();
		}
	}
}