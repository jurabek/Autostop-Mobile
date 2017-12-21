using Autostop.Client.Abstraction;
using Autostop.Client.Core.ViewModels.Passenger;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Autostop.Client.Mobile.UI.Pages.Pessengers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DestinationSearchPlacePage : ContentPage, IScreenFor<DestinationSearchPlaceViewModel>
    {
        public DestinationSearchPlacePage()
        {
            InitializeComponent();
        }

        public DestinationSearchPlaceViewModel ViewModel { get; set; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = ViewModel;
        }
    }
}