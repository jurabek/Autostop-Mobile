using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Autostop.Client.Android.Views;
using Autostop.Client.Mobile.UI.Pages.Pessengers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Autostop.Client.Android
{
    [Activity(Label = "Autostop.Client.Android", MainLauncher = true)]
    public class RootActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.root);

	        var toolbar = FindViewById<Toolbar>(Resource.Layout.toolbar);
			SetSupportActionBar(toolbar);

            Forms.Init(this, savedInstanceState);

            FragmentManager
	           .BeginTransaction()
               .Add(Resource.Id.container, new MainFragment())
	           .Commit();
        }
    }
}