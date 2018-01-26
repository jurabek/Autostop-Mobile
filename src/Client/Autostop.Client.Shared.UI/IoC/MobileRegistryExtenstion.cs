using Autofac;
using Autostop.Client.Abstraction;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Core.ViewModels.Passenger;
using Autostop.Client.Core.ViewModels.Passenger.LocationEditor;
using Autostop.Client.Core.ViewModels.Passenger.Welcome;
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
            builder.RegisterType<DestinationLocationEditorPage>().As<IScreenFor<DestinationLocationEditorViewModel>>();
            builder.RegisterType<PickupLocationEditorPage>().As<IScreenFor<PickupLocationEditorViewModel>>();
	        builder.RegisterType<HomeLocationEditorPage>().As<IScreenFor<HomeLocationEditorViewModel>>();
	        builder.RegisterType<WorkLocationEditorPage>().As<IScreenFor<WorkLocationEditorViewModel>>();
            builder.RegisterType<PhoneVerificationPage>().As<IScreenFor<PhoneVerificationViewModel>>();
			builder.RegisterType<SignInPage>().As<IScreenFor<SignInViewModel>>();
            builder.RegisterType<SettingsProvider>().As<ISettingsProvider>();
	        builder.RegisterInstance(CrossSettings.Current).As<ISettings>();
        }
    }
}