using Autostop.Client.Abstraction;
using Autostop.Client.Core.ViewModels.Passenger.LocationEditor;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Autostop.Client.Shared.UI.Pages.Pessengers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DestinationLocationEditorPage : ContentPage, IScreenFor<DestinationLocationEditorViewModel>
    {
        public DestinationLocationEditorPage()
        {
            InitializeComponent();
        }

        public DestinationLocationEditorViewModel ViewModel { get; set; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = ViewModel;
        }
    }
}