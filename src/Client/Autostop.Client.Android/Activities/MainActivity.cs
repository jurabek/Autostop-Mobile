using System;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Autofac;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Android.IoC;
using Autostop.Client.Core.ViewModels.Passenger;
using Xamarin.Forms;
using View = Android.Views.View;

namespace Autostop.Client.Android.Activities
{
	[Activity(Label = "Autostop", MainLauncher = true)]
	public class MainActivity : AppCompatActivity, FragmentManager.IOnBackStackChangedListener, View.IOnClickListener
	{
		private DrawerLayout _drawerLayout;
		private ActionBarDrawerToggle _drawerToggle;
		private Toolbar _toolbar;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.main);

			_toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetSupportActionBar(_toolbar);
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			SupportActionBar.SetDisplayShowTitleEnabled(false);

			_drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
			_drawerToggle = new ActionBarDrawerToggle(this, _drawerLayout, _toolbar, 0, 0);

			_drawerLayout.DrawerOpened += (_, __) => _drawerToggle.DrawerIndicatorEnabled = true;
			_drawerLayout.AddDrawerListener(_drawerToggle);
			_drawerToggle.SyncState();

			Forms.Init(this, savedInstanceState);
			AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
			TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

			var navigationService = new AndroidLocator()
				.Build()
				.Resolve<INavigationService>();

			navigationService.NavigateTo<MainViewModel>(true);
			FragmentManager.AddOnBackStackChangedListener(this);
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
			if (FragmentManager.BackStackEntryCount >= 1)
			{
				_drawerToggle.DrawerIndicatorEnabled = false;
				_drawerToggle.ToolbarNavigationClickListener = this;
			}
			else
			{
				_drawerToggle.DrawerIndicatorEnabled = true;
				_drawerToggle.ToolbarNavigationClickListener = _drawerToggle.ToolbarNavigationClickListener;
			}
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

		private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
		{
			//var newExc = new Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
		}


		private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
		{
			//var newExc = new Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as Exception);
		}
	}
}