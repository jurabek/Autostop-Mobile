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
using Autostop.Client.Core.ViewModels.Passenger;

namespace Autostop.Client.Android.Views
{
	public class MainFragment : Fragment, IScreenFor<MainViewModel>
	{
	    private MapView _mapView;
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
	        _mapView = view.FindViewById<MapView>(Resource.Id.mapView);
	    }

	    public MainViewModel ViewModel { get; set; }
	}
}