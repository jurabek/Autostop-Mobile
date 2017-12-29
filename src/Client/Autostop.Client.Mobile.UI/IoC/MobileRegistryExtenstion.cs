using Autofac;
using Autostop.Client.Abstraction;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Core.ViewModels.Passenger.Places;
using Autostop.Client.Mobile.UI.Pages.Pessengers;
using Autostop.Client.Mobile.UI.Providers;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Autostop.Client.Mobile.UI.IoC
{
    public static class MobileRegistryExtenstion
    {
        public static void ClientTypesRegistry(this ContainerBuilder builder)
        {
            builder.RegisterType<DestinationSearchPlacePage>().As<IScreenFor<DestinationSearchPlaceViewModel>>();
            builder.RegisterType<PickupSearchPlacePage>().As<IScreenFor<PickupSearchPlaceViewModel>>();
	        builder.RegisterType<SearchHomeAddressPage>().As<IScreenFor<SearchHomeAddressViewModel>>();
	        builder.RegisterType<SearchWorkAddressPage>().As<IScreenFor<SearchWorkAddressViewModel>>();
			builder.RegisterType<SettingsProvider>().As<ISettingsProvider>();
	        builder.RegisterInstance(CrossSettings.Current).As<ISettings>();
        }
    }
}