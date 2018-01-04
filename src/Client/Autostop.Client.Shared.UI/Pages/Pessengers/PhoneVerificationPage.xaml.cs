using Autostop.Client.Abstraction;
using Autostop.Client.Core.ViewModels.Passenger;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Autostop.Client.Shared.UI.Pages.Pessengers
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PhoneVerificationPage : ContentPage, IScreenFor<PhoneVerificationViewModel>
	{
	    private PhoneVerificationViewModel _viewModel;

	    public PhoneVerificationPage ()
		{
			InitializeComponent();
		}

	    public PhoneVerificationViewModel ViewModel
	    {
	        get => _viewModel;
	        set
	        {
	            _viewModel = value;
	            this.BindingContext = value;
	        }
	    }
	}
}