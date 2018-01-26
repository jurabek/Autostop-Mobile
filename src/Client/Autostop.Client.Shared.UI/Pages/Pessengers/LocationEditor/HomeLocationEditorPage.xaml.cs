using Autostop.Client.Abstraction;
using Autostop.Client.Core.ViewModels.Passenger.LocationEditor;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Autostop.Client.Shared.UI.Pages.Pessengers
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomeLocationEditorPage : ContentPage, IScreenFor<HomeLocationEditorViewModel>
	{
		public HomeLocationEditorPage ()
		{
			InitializeComponent ();
		}

		public HomeLocationEditorViewModel ViewModel { get; set; }

		protected override void OnAppearing()
		{
			base.OnAppearing();
			BindingContext = ViewModel;
		}
	}
}