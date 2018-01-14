using System;
using System.Threading.Tasks;
using Android.App;
using Android.Gms.Common;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Widget;
using Autofac;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Android.Platform.Android.IoC;
using Autostop.Client.Core.ViewModels.Passenger;
using Xamarin.Forms;
using SearchView = Android.Support.V7.Widget.SearchView;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using View = Android.Views.View;

namespace Autostop.Client.Android.Activities
{
	[Activity(Label = "Autostop", MainLauncher = true)]
	public class MainActivity : AppCompatActivity, FragmentManager.IOnBackStackChangedListener, View.IOnClickListener
	{
		private DrawerLayout _drawerLayout;
		private ActionBarDrawerToggle _drawerToggle;
		private Toolbar _toolbar;
		internal TextView TitleTextView { get; private set; }
		internal SearchView SearchView { get; private set; }

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.main);
			CheckGooglePlayServicesIsInstalled();
			TitleTextView = FindViewById<TextView>(Resource.Id.toolbarTitleTextView);
			SearchView = FindViewById<SearchView>(Resource.Id.toolbarSearchView);

			_toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetSupportActionBar(_toolbar);
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			SupportActionBar.SetDisplayShowTitleEnabled(false);

			_drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
			_drawerToggle = new ActionBarDrawerToggle(this, _drawerLayout, _toolbar, 0, 0);

			_drawerLayout.DrawerOpened += (_, __) => _drawerToggle.DrawerIndicatorEnabled = true;
			_drawerLayout.AddDrawerListener(_drawerToggle);
			_drawerToggle.SyncState();
			FragmentManager.AddOnBackStackChangedListener(this);

			Forms.Init(this, savedInstanceState);
			new AndroidLocator()
				.Build()
				.Resolve<INavigationService>()
				.NavigateTo<MainViewModel>(true);

		}
		public void OnBackStackChanged()
		{
			SyncDrawerToggleState();
		}

		public void OnClick(View v)
		{
			FragmentManager.PopBackStackImmediate();
		}

		private void SyncDrawerToggleState()
		{
			var condition = FragmentManager.BackStackEntryCount < 1;
			_drawerToggle.DrawerIndicatorEnabled = condition;
			_drawerToggle.ToolbarNavigationClickListener = condition ? _drawerToggle.ToolbarNavigationClickListener : this;
		}

		public override void OnBackPressed()
		{
			if (_drawerLayout.IsDrawerOpen(GravityCompat.Start))
			{
				_drawerLayout.CloseDrawer(GravityCompat.Start);
			}
			else
			{
				base.OnBackPressed();
			}
		}

		private void CheckGooglePlayServicesIsInstalled()
		{
			int queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
			if (queryResult == ConnectionResult.Success)
			{
				Log.Info("", "Google Play Services is installed on this device.");
			}
			else
			{
				throw new GooglePlayServicesNotAvailableException(0);
			}
		}
	}
}