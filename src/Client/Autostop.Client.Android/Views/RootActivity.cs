using System;
using System.Threading.Tasks;
using Android.App;
using Android.Locations;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Autofac;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Android.IoC;
using Autostop.Client.Android.Managers;
using Autostop.Client.Core.ViewModels.Passenger;
using Autostop.Client.Shared.UI.Pages.Pessengers;
using Firebase;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Autostop.Client.Android.Views
{
	[Activity(Label = "Autostop Android", MainLauncher = true)]
	public class RootActivity : AppCompatActivity
	{
		public static RootActivity Instance { get; private set; }
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			Forms.Init(this, savedInstanceState);
			SetContentView(Resource.Layout.root);
			Instance = this;
			
			var toolbar = FindViewById<Toolbar>(Resource.Layout.toolbar);
			SetSupportActionBar(toolbar);

			AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
			TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

			new AndroidLocator()
				.Build()
				.Resolve<INavigationService>()
				.NavigateTo<MainViewModel>();
		}

		private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
		{
			var newExc = new Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
		}


		private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
		{
			var newExc = new Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as Exception);
		}
	}
}