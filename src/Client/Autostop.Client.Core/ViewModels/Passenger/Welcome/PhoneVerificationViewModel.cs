using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Autostop.Client.Abstraction.Providers;
using Autostop.Common.Shared.Models;
using GalaSoft.MvvmLight.Command;
using JetBrains.Annotations;

namespace Autostop.Client.Core.ViewModels.Passenger.Welcome
{
    [UsedImplicitly]
    public class PhoneVerificationViewModel : BaseViewModel
    {
        private readonly IPhoneAuthenticationProvider _phoneAuthenticationProvider;

        public PhoneVerificationViewModel(IPhoneAuthenticationProvider phoneAuthenticationProvider)
        {
            _phoneAuthenticationProvider = phoneAuthenticationProvider;
            Countries = new ObservableCollection<VerificationCountryCode>
            {
                new VerificationCountryCode
                {
                    CountryCodeFormatted = "(+992)",
                    CountryCode = "+992",
                    CountryName = "Tajikistan"
                },
                new VerificationCountryCode
                {
                    CountryCodeFormatted = "(+998)",
                    CountryCode = "+998",
                    CountryName = "Uzbekistan"
                },
                new VerificationCountryCode
                {
                    CountryCodeFormatted = "(+371)",
                    CountryCode = "+371",
                    CountryName = "Latvia"
                }
            };
			
        }

        public ObservableCollection<VerificationCountryCode> Countries { get; }

        private VerificationCountryCode _selectedCountry;

        public VerificationCountryCode SelectedCountry
        {
            get => _selectedCountry;
	        set => RaiseAndSetIfChanged(ref _selectedCountry, value);
        }

        private string _phoneNumber;

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => RaiseAndSetIfChanged(ref _phoneNumber, value);
        }

	    private ICommand _verifyCommand;
        public ICommand VerifyCommand => _verifyCommand ?? (_verifyCommand = new RelayCommand(async () =>
			{
				try
				{
					var result = await _phoneAuthenticationProvider.VerifyPhoneNumber(SelectedCountry.CountryCode + PhoneNumber);
				}
				catch (Exception e)
				{
				}
			}));
	}
}
