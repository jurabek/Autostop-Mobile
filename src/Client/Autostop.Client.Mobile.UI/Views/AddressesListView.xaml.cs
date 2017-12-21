using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Autostop.Client.Mobile.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddressesListView : ListView
    {
        public AddressesListView() : base(ListViewCachingStrategy.RecycleElementAndDataTemplate)
        {
            InitializeComponent();
        }
    }
}