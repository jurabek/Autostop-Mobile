using Autostop.Client.Abstraction;
using Autostop.Client.Core.ViewModels.Passenger.Places;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Autostop.Client.Shared.UI.Pages.Pessengers
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SearchWorkAddressPage : ContentPage, IScreenFor<SearchWorkAddressViewModel>
	{
		public SearchWorkAddressPage ()
		{
			InitializeComponent ();
		}

		public SearchWorkAddressViewModel ViewModel { get; set; }

		protected override void OnAppearing()
		{
			base.OnAppearing();
			BindingContext = ViewModel;
		}
	}
}