using Android.App;
using Android.OS;
using Autostop.Client.Android.Resources;

namespace Autostop.Client.Android
{
    [Activity(Label = "Autostop.Client.Android", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
			
        }
    }
}