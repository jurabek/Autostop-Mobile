using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Autostop.Client.Mobile.UI.Pages.Pessengers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Autostop.Client.Android
{
    [Activity(Label = "Autostop.Client.Android", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            Forms.Init(this, savedInstanceState);

            FragmentManager.BeginTransaction()
               .Replace(Resource.Id.container, new PhoneVerificationPage().CreateFragment(this), "main").Commit();
        }
    }
}