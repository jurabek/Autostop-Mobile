using Autostop.Client.Abstraction;
using Autostop.Client.Core.ViewModels.Passenger.LocationEditor;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Autostop.Client.Shared.UI.Pages.Pessengers.LocationEditor
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WorkLocationEditorPage : ContentPage, IScreenFor<WorkLocationEditorViewModel>
	{
		public WorkLocationEditorPage ()
		{
			InitializeComponent ();
		}

		public WorkLocationEditorViewModel ViewModel { get; set; }

		protected override void OnAppearing()
		{
			base.OnAppearing();
			BindingContext = ViewModel;
		}
	}
}