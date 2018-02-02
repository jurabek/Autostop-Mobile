using Autostop.Client.Abstraction;
using Autostop.Client.Core.ViewModels.Passenger.LocationEditor;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Autostop.Client.Shared.UI.Pages.Pessengers.LocationEditor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PickupLocationEditorPage : ContentPage, IScreenFor<PickupLocationEditorViewModel>
    {
        public PickupLocationEditorPage()
        {
            InitializeComponent();
        }

        public PickupLocationEditorViewModel ViewModel { get; set; }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = ViewModel;
            await ViewModel.Load();
        }
    }
}