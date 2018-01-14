using System;
using System.Reactive;
using System.Reactive.Linq;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Autostop.Client.Abstraction;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Android.Activities;
using Autostop.Client.Android.Extensions;
using Autostop.Client.Android.Platform.Android.Abstraction;
using Autostop.Client.Core.ViewModels.Passenger;
using GalaSoft.MvvmLight.Helpers;
using LatLng = Android.Gms.Maps.Model.LatLng;
using Location = Autostop.Common.Shared.Models.Location;
using MapView = Android.Gms.Maps.MapView;

namespace Autostop.Client.Android.Fragments
{
	public class MainFragment : Fragment, IScreenFor<MainViewModel>, IOnMapReadyCallback
	{
		private readonly IVisibleRegionProvider _visibleRegionProvider;
		private readonly IMarkerAdapter _markerAdapter;
		private MapView _mapView;
		private EditText _pickupAddressEditText;
		private Button _whereToGoButton;
		private ImageView _centeredMarkerImage;
		private GoogleMap _googleMap;
		private ProgressBar _pickupAddressLoading;

		public MainFragment(
			IVisibleRegionProvider visibleRegionProvider,
			IMarkerAdapter markerAdapter)
		{
			_visibleRegionProvider = visibleRegionProvider;
			_markerAdapter = markerAdapter;
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			return inflater.Inflate(Resource.Layout.main_fragment, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState)
		{
			base.OnViewCreated(view, savedInstanceState);

			_pickupAddressEditText = view.FindViewById<EditText>(Resource.Id.pickupLocationAddressEditText);
			_whereToGoButton = view.FindViewById<Button>(Resource.Id.whereToGoButton);
			_centeredMarkerImage = view.FindViewById<ImageView>(Resource.Id.centeredMarkerIcon);
			_pickupAddressLoading = view.FindViewById<ProgressBar>(Resource.Id.pickupAddressLoading);

			_mapView = view.FindViewById<MapView>(Resource.Id.mapView);
			_mapView.OnCreate(savedInstanceState);
			_mapView.OnResume();
			_mapView.GetMapAsync(this);

			_pickupAddressEditText.SetCommand(nameof(EditText.Click), ViewModel.NavigateToPickupSearch);
			_whereToGoButton.SetCommand(nameof(Button.Click), ViewModel.NavigateToDestinationSearch);

			this.SetBinding(() => ViewModel.RideViewModel.IsPickupAddressLoading)
				.WhenSourceChanges(() =>
				{
					_pickupAddressLoading.Visibility =
						ViewModel.RideViewModel.IsPickupAddressLoading ? ViewStates.Visible : ViewStates.Gone;
				});

			this.SetBinding(() => ViewModel.CameraTarget)
				.WhenSourceChanges(() =>
				{
					var camera = CameraUpdateFactory.NewLatLngZoom(new LatLng(ViewModel.CameraTarget.Latitude, ViewModel.CameraTarget.Longitude), 17);
					_googleMap?.MoveCamera(camera);
				});

			this.SetBinding(() => ViewModel.OnlineDrivers)
				.WhenSourceChanges(async () =>
				{
					if(_googleMap == null)
						return;

					var bitmapSource = await BitmapFactory.DecodeResourceAsync(Resources, Resource.Drawable.car);
					var bitmap = Bitmap.CreateScaledBitmap(bitmapSource, 20, 40, false);
					var icon = BitmapDescriptorFactory.FromBitmap(bitmap);
					foreach (var onlineDriver in ViewModel.OnlineDrivers)
					{
						_googleMap.AddMarker(_markerAdapter.GetMarkerOptions(onlineDriver).SetIcon(icon));
					}
				});

			this.SetBinding(
				() => _pickupAddressEditText.Text,
				() => ViewModel.RideViewModel.PickupAddress.FormattedAddress, BindingMode.TwoWay);
		}

		public MainViewModel ViewModel { get; set; }

		public async void OnMapReady(GoogleMap googleMap)
		{
			_googleMap = googleMap;
			_googleMap.MyLocationEnabled = true;
			_googleMap.UiSettings.CompassEnabled = true;
			_googleMap.UiSettings.MyLocationButtonEnabled = false;

			var cameraPositionIdle = Observable
				.FromEventPattern<EventHandler, EventArgs>(
					e => _googleMap.CameraIdle += e,
					e => _googleMap.CameraIdle -= e);

			ViewModel.CameraPositionObservable = cameraPositionIdle
				.Select(_ => _googleMap.CameraPosition.Target)
				.Select(target => new Location(target.Latitude, target.Longitude));

			ViewModel.VisibleRegionChanged = cameraPositionIdle
				.Select(_ => _googleMap.Projection.VisibleRegion.LatLngBounds)
				.Select(bounds => _visibleRegionProvider.GetVisibleRegion(bounds.Northeast.ToLocation(), bounds.Southwest.ToLocation()));

			ViewModel.CameraStartMoving = Observable
				.FromEventPattern<EventHandler<GoogleMap.CameraMoveStartedEventArgs>, GoogleMap.CameraMoveStartedEventArgs>(
					e => _googleMap.CameraMoveStarted += e,
					e => _googleMap.CameraMoveStarted -= e)
				.Do(CameraStarted)
				.Select(e => true);

			await ViewModel.Load();
		}

		private void CameraStarted(EventPattern<GoogleMap.CameraMoveStartedEventArgs> eventPattern)
		{
		}

		public override void OnResume()
		{
			base.OnResume();
			_mapView.OnResume();
			var activity = ((MainActivity)Activity);
			activity.TitleTextView.Visibility = ViewStates.Visible;
			activity.SearchView.Visibility = ViewStates.Gone;
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