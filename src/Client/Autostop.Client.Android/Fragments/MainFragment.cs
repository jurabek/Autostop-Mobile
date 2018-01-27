using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Autostop.Client.Abstraction;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Android.Abstraction;
using Autostop.Client.Android.Activities;
using Autostop.Client.Android.Extensions;
using Autostop.Client.Core.Extensions;
using Autostop.Client.Core.ViewModels.Passenger;
using GalaSoft.MvvmLight.Helpers;
using LatLng = Android.Gms.Maps.Model.LatLng;
using Location = Autostop.Common.Shared.Models.Location;
using MapView = Android.Gms.Maps.MapView;

namespace Autostop.Client.Android.Fragments
{
	public class MainFragment : Fragment, IScreenFor<MainViewModel>, IOnMapReadyCallback
	{
		private readonly ISchedulerProvider _schedulerProvider;
		private readonly IVisibleRegionProvider _visibleRegionProvider;
		private readonly IMarkerAdapter _markerAdapter;
		private readonly IMarkerSizeProvider _markerSizeProvider;
		private MapView _mapView;
		private EditText _pickupAddressEditText;
		private Button _whereToGoButton;
		private GoogleMap _googleMap;
		private ProgressBar _pickupAddressLoading;
		private TextView _estimatedTimeTimeTextView;
		private ImageView _centeredAnimatableDot;
		private View _requestView;

		public MainFragment(
			ISchedulerProvider schedulerProvider,
			IVisibleRegionProvider visibleRegionProvider,
			IMarkerAdapter markerAdapter,
			IMarkerSizeProvider markerSizeProvider)
		{
			_schedulerProvider = schedulerProvider;
			_visibleRegionProvider = visibleRegionProvider;
			_markerAdapter = markerAdapter;
			_markerSizeProvider = markerSizeProvider;
		}

		public override async void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			await ViewModel.GetMyLocation();
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) => 
			inflater.Inflate(Resource.Layout.main_fragment, container, false);

		public override void OnViewCreated(View view, Bundle savedInstanceState)
		{
			base.OnViewCreated(view, savedInstanceState);
			_pickupAddressEditText = view.FindViewById<EditText>(Resource.Id.pickupLocationAddressEditText);
			_whereToGoButton = view.FindViewById<Button>(Resource.Id.whereToGoButton);
			_pickupAddressLoading = view.FindViewById<ProgressBar>(Resource.Id.pickupAddressLoading);
			_estimatedTimeTimeTextView = view.FindViewById<TextView>(Resource.Id.estimatedTimeTextView);
			_centeredAnimatableDot = view.FindViewById<ImageView>(Resource.Id.centeredAnimatableDot);
			_requestView = view.FindViewById<RelativeLayout>(Resource.Id.requestView);

			_mapView = view.FindViewById<MapView>(Resource.Id.mapView);
			_mapView.OnCreate(savedInstanceState);
			_mapView.OnResume();
			_mapView.GetMapAsync(this);

			_pickupAddressEditText.SetCommand(nameof(EditText.Click), ViewModel.TripLocationViewModel.NavigateToPickupSearch);
			_whereToGoButton.SetCommand(nameof(Button.Click), ViewModel.TripLocationViewModel.NavigateToWhereTo);
		}

		public MainViewModel ViewModel { get; set; }

		public void slideUp(View view)
		{
			view.Visibility = ViewStates.Visible;

			TranslateAnimation animate = new TranslateAnimation(0, 0, view.Height, 0)
			{
				Duration = 500,
				FillAfter = true
			};
			view.StartAnimation(animate);

			view.BringToFront();
			view.Parent.RequestLayout();
		}

		public void slideDown(View view)
		{
			TranslateAnimation animate = new TranslateAnimation(0, 0, 0, view.Height)
			{
				Duration = 500,
				FillAfter = true
			};
			view.StartAnimation(animate);
		}

		public async void OnMapReady(GoogleMap googleMap)
		{
			_googleMap = googleMap;
			_googleMap.MyLocationEnabled = true;
			_googleMap.UiSettings.CompassEnabled = true;
			_googleMap.UiSettings.MyLocationButtonEnabled = true;
			MoveMyLocationButtonIntoBottom();

			var cameraPositionIdle = Observable
				.FromEventPattern<EventHandler, EventArgs>(
					e => _googleMap.CameraIdle += e,
					e => _googleMap.CameraIdle -= e);

			ViewModel.CameraPositionObservable = cameraPositionIdle
				.Do(CamerPositionIdle)
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

			this.SetBinding(() => ViewModel.TripLocationViewModel.PickupAddress.Loading)
				.WhenSourceChanges(() =>
				{
					_pickupAddressLoading.Visibility =
						ViewModel.TripLocationViewModel.PickupAddress.Loading ? ViewStates.Visible : ViewStates.Gone;
				});

			this.SetBinding(
				() => _pickupAddressEditText.Text,
				() => ViewModel.TripLocationViewModel.PickupAddress.FormattedAddress, BindingMode.TwoWay);

			ViewModel.Changed(() => ViewModel.OnlineDrivers)
				.Where(od => _googleMap != null && od.Any())
				.SubscribeOn(_schedulerProvider.SynchronizationContextScheduler)
				.Subscribe(async od =>
				{
					var zoomLevel = _googleMap.CameraPosition.Zoom;
					var width = _markerSizeProvider.GetWidth(zoomLevel);
					var height = _markerSizeProvider.GetHeight(zoomLevel);

					var bitmapSource = await BitmapFactory.DecodeResourceAsync(Resources, Resource.Drawable.car);
					var bitmap = Bitmap.CreateScaledBitmap(bitmapSource, width, height, false);
					var icon = BitmapDescriptorFactory.FromBitmap(bitmap);
					_googleMap.Clear();
					foreach (var onlineDriver in ViewModel.OnlineDrivers)
					{
						_googleMap.AddMarker(_markerAdapter.GetMarkerOptions(onlineDriver).SetIcon(icon));
					}
				});

			this.SetBinding(() => ViewModel.TripLocationViewModel.CanRequest)
				.WhenSourceChanges(() =>
				{
					if (ViewModel.TripLocationViewModel.CanRequest)
					{
						slideUp(_requestView);
						_whereToGoButton.Visibility = ViewStates.Gone;
					}
					else
					{
						slideDown(_requestView);
						_whereToGoButton.Visibility = ViewStates.Visible;
					}
				});

					this.SetBinding(() => ViewModel.CameraTarget, BindingMode.TwoWay)
				.WhenSourceChanges(() =>
				{
					var camera = CameraUpdateFactory.NewLatLngZoom(new LatLng(ViewModel.CameraTarget.Latitude, ViewModel.CameraTarget.Longitude), 17);
					_googleMap?.MoveCamera(camera);
				});

			await ViewModel.Load();
		}

		private void MoveMyLocationButtonIntoBottom()
		{
			if (_mapView.FindViewById(1) != null)
			{
				View locationButton = ((View)_mapView.FindViewById(1).Parent).FindViewById(2);
				RelativeLayout.LayoutParams layoutParams = (RelativeLayout.LayoutParams)locationButton.LayoutParameters;
				layoutParams.AddRule(LayoutRules.AlignParentTop, 0);
				layoutParams.AddRule(LayoutRules.AlignParentBottom, 1);
				DisplayMetrics realMetrics = new DisplayMetrics();
				Activity.WindowManager.DefaultDisplay.GetRealMetrics(realMetrics);
				var bottomPadding = realMetrics.HeightPixels / 4;
				layoutParams.SetMargins(0, 0, 30, bottomPadding);
			}
		}

		private void CamerPositionIdle(EventPattern<EventArgs> eventPattern)
		{
			_estimatedTimeTimeTextView.Visibility = ViewStates.Visible;
			Random rnd = new Random();
			var min = rnd.Next(0, 10);
			_estimatedTimeTimeTextView.Text = $"{min}\nmin";
			_centeredAnimatableDot.Visibility = ViewStates.Gone;
		}

		private void CameraStarted(EventPattern<GoogleMap.CameraMoveStartedEventArgs> eventPattern)
		{
			_estimatedTimeTimeTextView.Visibility = ViewStates.Gone;
			_centeredAnimatableDot.Visibility = ViewStates.Visible;
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