using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Autostop.Client.Mobile.UI.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddressesListView : ListView
	{
		public AddressesListView () : base(ListViewCachingStrategy.RecycleElementAndDataTemplate)
		{
			InitializeComponent ();
		}
	}
}