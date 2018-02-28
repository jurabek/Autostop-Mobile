using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Autostop.Client.Android.Fragments
{
	public abstract class BaseChooseOnMapFragment<TViewModel> : Fragment, IScreenFor<TViewModel>, IOnMapReadyCallback
		where TViewModel : class, ISearchableViewModel, IMapViewModel
	{
		private EditText _addressEditText;
		private ProgressBar _pickupAddressLoading;
		private MapView _mapView;

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState)
		{
			base.OnViewCreated(view, savedInstanceState);
			_mapView = view.FindViewById<MapView>(Resource.Id.choseOnMapFragmentMapView);
			_addressEditText = view.FindViewById<EditText>(Resource.Id.selecedAddressText);
			_pickupAddressLoading = view.FindViewById<ProgressBar>(Resource.Id.selectedAddressLoading);

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
			throw new NotImplementedException();
		}
	}
}