using Android.App;
using Android.Gms.Common;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Util;
using Android.Views;
using Autostop.Client.Abstraction;
using Autostop.Client.Core.ViewModels.Passenger;
using JetBrains.Annotations;

namespace Autostop.Client.Android.Views
{
	[UsedImplicitly]
	public class MainFragment : Fragment, IScreenFor<MainViewModel>, IOnMapReadyCallback
	{
		private MapView _mapView;
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

			_mapView = view.FindViewById<MapView>(Resource.Id.mapView);
			_mapView.OnCreate(savedInstanceState);
			_mapView.OnResume();
			_mapView.GetMapAsync(this);
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

		public void OnMapReady(GoogleMap googleMap)
		{
			LatLng sydney = new LatLng(38.565558, 68.799828);
			_googleMap = googleMap;
			_googleMap.MyLocationEnabled = true;
			_googleMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(sydney, 17));
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