using Autostop.Client.Abstraction;
using Autostop.Client.Core.ViewModels.Passenger;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Autostop.Client.Mobile.UI.Pages.Pessengers
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PickupSearchPlacePage : ContentPage, IScreenFor<PickupSearchPlaceViewModel>
	{
		public PickupSearchPlacePage ()
		{
			InitializeComponent ();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			BindingContext = ViewModel;
		}

		public PickupSearchPlaceViewModel ViewModel { get; set; }
	}
}