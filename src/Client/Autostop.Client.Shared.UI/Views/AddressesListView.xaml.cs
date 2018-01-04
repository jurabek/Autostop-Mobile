using System;
using System.Reactive.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Autostop.Client.Shared.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddressesListView : ListView
    {
        public AddressesListView() : base(ListViewCachingStrategy.RecycleElementAndDataTemplate)
        {
            InitializeComponent();
	        Observable.FromEventPattern<SelectedItemChangedEventArgs>(this, "ItemSelected")
		        .Select(x => x.Sender)
		        .Cast<ListView>()
		        .Subscribe(l => l.SelectedItem = null);
		}
    }
}