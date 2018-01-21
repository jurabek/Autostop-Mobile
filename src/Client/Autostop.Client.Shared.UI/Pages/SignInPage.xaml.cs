using Autostop.Client.Abstraction;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Core.ViewModels.Passenger;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Autostop.Client.Shared.UI.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignInPage : ContentPage, IScreenFor<SignInViewModel>
	{
		private readonly IKeyboardProvider keyboardProvider;
		private View _currentView;

		public SignInPage(IKeyboardProvider keyboardProvider)
		{
			InitializeComponent();
			LoginButton.Clicked += LoginButton_Clicked;
			this.keyboardProvider = keyboardProvider;
			this.keyboardProvider.KeyboardOpened += KeyboardProvider_KeyboardOpened;
			this.keyboardProvider.KeyboardClosed += KeyboardProvider_KeyboardClosed;
			_currentView = LoginView;
		}

		private async void KeyboardProvider_KeyboardClosed(object sender, EventArgs e)
		{
			await _currentView.TranslateTo(0, 0);
		}

		private async void KeyboardProvider_KeyboardOpened(object sender, EventArgs e)
		{
			await _currentView.TranslateTo(0, -_currentView.Height);
		}

		private async void LoginButton_Clicked(object sender, EventArgs e)
		{
			await LoginView.ScaleTo(0);
			LoginView.IsVisible = false;

			ConfirmCodeView.IsVisible = true;
			await ConfirmCodeView.ScaleTo(1);

			_currentView = ConfirmCodeView;
		}

		public SignInViewModel ViewModel { get; set; }
	}
}