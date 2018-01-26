using Android.App;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Core.IoC;
using Autostop.Client.Core.ViewModels.Passenger;
using Plugin.Permissions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using SearchView = Android.Support.V7.Widget.SearchView;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using View = Android.Views.View;

namespace Autostop.Client.Android.Activities
{
	[Activity(Label = "Autostop", MainLauncher = true)]
	public class MainActivity :
		AppCompatActivity,
		FragmentManager.IOnBackStackChangedListener,
		View.IOnClickListener,
		ViewTreeObserver.IOnGlobalLayoutListener
	{
		private DrawerLayout _drawerLayout;
		private ActionBarDrawerToggle _drawerToggle;
		private Toolbar _toolbar;
		private IKeyboardProvider _keyboardProvider;
		private INavigationService _navigationService;
		private bool keyboardOpened;

		internal TextView TitleTextView { get; private set; }
		internal SearchView SearchView { get; private set; }

		protected override async void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			_keyboardProvider = Locator.Resolve<IKeyboardProvider>();
			_navigationService = Locator.Resolve<INavigationService>();

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
			_drawerLayout.ViewTreeObserver.AddOnGlobalLayoutListener(this);
			_drawerLayout.AddDrawerListener(_drawerToggle);
			_drawerToggle.SyncState();

			FragmentManager.AddOnBackStackChangedListener(this);
			Forms.Init(this, savedInstanceState);
			await CheckPermission();

			_navigationService.NavigateTo<MainViewModel>(true);
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

		public void OnGlobalLayout()
		{
			Rect displayFrame = new Rect();
			_drawerLayout.GetWindowVisibleDisplayFrame(displayFrame);
			int screenHeight = _drawerLayout.RootView.Height;
			int keypadHeight = screenHeight - displayFrame.Bottom;
			_keyboardProvider.ScreenHeight = screenHeight;

			Log.Debug("", "keypadHeight = " + keypadHeight);
			if (keypadHeight > screenHeight * 0.15)
			{
				keyboardOpened = true;
				_keyboardProvider.KeyboardHeight = keypadHeight;
				_keyboardProvider.OnKeyboardOpened();
			}
			else
			{
				if (keyboardOpened)
				{
					_keyboardProvider.KeyboardHeight = 0;
					_keyboardProvider.OnKeyboardClosed();
					keyboardOpened = false;
				}
			}
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
		{
			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

		private async Task CheckPermission()
		{
			try
			{
				var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Location);
				if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
				{
					if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Plugin.Permissions.Abstractions.Permission.Location))
					{
						Log.Debug("", "Need location", "Need that location", "OK");
					}

					var results = await CrossPermissions.Current.RequestPermissionsAsync(Plugin.Permissions.Abstractions.Permission.Location);
					//Best practice to always check that the key exists
					if (results.ContainsKey(Plugin.Permissions.Abstractions.Permission.Location))
						status = results[Plugin.Permissions.Abstractions.Permission.Location];
				}
				
			}
			catch (Exception ex)
			{
				throw;
			}
		}
	}
}