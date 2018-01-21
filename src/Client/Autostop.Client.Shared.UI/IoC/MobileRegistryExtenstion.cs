using Autofac;
using Autostop.Client.Abstraction;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Core.ViewModels.Passenger;
using Autostop.Client.Core.ViewModels.Passenger.Places;
using Autostop.Client.Shared.UI.Pages;
using Autostop.Client.Shared.UI.Pages.Pessengers;
using Autostop.Client.Shared.UI.Providers;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Autostop.Client.Shared.UI.IoC
{
    public static class MobileRegistryExtenstion
    {
        public static void ClientTypesRegistry(this ContainerBuilder builder)
        {
            builder.RegisterType<DestinationSearchPlacePage>().As<IScreenFor<DestinationSearchPlaceViewModel>>();
            builder.RegisterType<PickupSearchPlacePage>().As<IScreenFor<PickupSearchPlaceViewModel>>();
	        builder.RegisterType<SearchHomeAddressPage>().As<IScreenFor<SearchHomeAddressViewModel>>();
	        builder.RegisterType<SearchWorkAddressPage>().As<IScreenFor<SearchWorkAddressViewModel>>();
            builder.RegisterType<PhoneVerificationPage>().As<IScreenFor<PhoneVerificationViewModel>>();
			builder.RegisterType<SignInPage>().As<IScreenFor<SignInViewModel>>();
            builder.RegisterType<SettingsProvider>().As<ISettingsProvider>();
	        builder.RegisterInstance(CrossSettings.Current).As<ISettings>();
        }
    }
}