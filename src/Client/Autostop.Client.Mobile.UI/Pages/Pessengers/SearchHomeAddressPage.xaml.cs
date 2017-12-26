using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autostop.Client.Abstraction;
using Autostop.Client.Core.ViewModels.Passenger.Places;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Autostop.Client.Mobile.UI.Pages.Pessengers
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SearchHomeAddressPage : ContentPage, IScreenFor<SearchHomeAddressViewModel>
	{
		public SearchHomeAddressPage ()
		{
			InitializeComponent ();
		}

		public SearchHomeAddressViewModel ViewModel { get; set; }

		protected override void OnAppearing()
		{
			base.OnAppearing();
			BindingContext = ViewModel;
		}
	}
}