using Android.App;
using Android.Gms.Common;
using Android.Gms.Maps;
using Android.OS;
using Android.Util;
using Android.Views;
using Autostop.Client.Abstraction;
using Autostop.Client.Core.ViewModels.Passenger;
using System;
using System.Reactive;
using System.Reactive.Linq;
using Android.Widget;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Android.Extensions;
using GalaSoft.MvvmLight.Helpers;
using LatLng = Android.Gms.Maps.Model.LatLng;
using Location = Autostop.Common.Shared.Models.Location;
using MapView = Android.Gms.Maps.MapView;

namespace Autostop.Client.Android.Views
{
	public class MainFragment : Fragment, IScreenFor<MainViewModel>, IOnMapReadyCallback
	{
	    private readonly IVisibleRegionProvider _visibleRegionProvider;
	    private MapView _mapView;
		private EditText _pickupAddressEditText;
	    private Button _whereToGoButton;
	    private ImageView _centeredMarkerImage;
		private GoogleMap _googleMap;

	    public MainFragment(IVisibleRegionProvider visibleRegionProvider)
	    {
	        _visibleRegionProvider = visibleRegionProvider;
	    }
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
		    _whereToGoButton = view.FindViewById<Button>(Resource.Id.whereToGoButton);
		    _centeredMarkerImage = view.FindViewById<ImageView>(Resource.Id.centeredMarkerIcon);
            
            _pickupAddressEditText.SetTextSize(ComplexUnitType.Dip, 12);
            _centeredMarkerImage.SetX(-15);

			_mapView = view.FindViewById<MapView>(Resource.Id.mapView);
			_mapView.OnCreate(savedInstanceState);
			_mapView.OnResume();
			_mapView.GetMapAsync(this);

			_pickupAddressEditText.SetCommand(nameof(EditText.Click), ViewModel.NavigateToPickupSearch);
            _whereToGoButton.SetCommand(nameof(Button.Click), ViewModel.NavigateToDestinationSearch);

			this.SetBinding(() => ViewModel.CameraTarget)
				.WhenSourceChanges(() =>
				{
					var camera = CameraUpdateFactory.NewLatLngZoom(new LatLng(ViewModel.CameraTarget.Latitude, ViewModel.CameraTarget.Longitude), 17);
					_googleMap?.MoveCamera(camera);
				});

		    this.SetBinding(() => ViewModel.OnlineDrivers)
		        .WhenSourceChanges(() =>
		        {
                    //BitmapDescriptor icon = BitmapDescriptorFactory.FromResource(Resource.Drawable.car);

                    //foreach (var driverLocation in ViewModel.OnlineDrivers)
                    //{
                    //    var markerOption = new MarkerOptions();
                    //    markerOption.SetPosition(new LatLng(driverLocation.CurrentLocation.Latitude, driverLocation.CurrentLocation.Longitude));
                    //    markerOption.Anchor((float)0.5, (float)0.5);
                    //    markerOption.SetRotation((float)driverLocation.Bearing);
                    //    markerOption.Flat(true);
                    //    markerOption.SetIcon(icon);

                    //    _googleMap.AddMarker(markerOption);
                    //}
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
	        //ScaleAnimation fadeIn =
	        //    new ScaleAnimation(0f, 1f, 0f, 1f, Dimension.RelativeToSelf, 0.5f, Dimension.RelativeToSelf, 0.5f)
	        //    {
	        //        Duration = 1,
	        //        FillAfter = true,
         //           RepeatMode = RepeatMode.Reverse
	        //    };
	        //// animation duration in milliseconds
	        //// If fillAfter is true, the transformation that this animation performed will persist when it is finished.
	        //_pickupLocationbutton.StartAnimation(fadeIn);
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