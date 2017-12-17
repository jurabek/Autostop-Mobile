using Autofac;
using Autostop.Client.Abstraction;
using Autostop.Client.Core.ViewModels.Passenger;
using Autostop.Client.Mobile.UI.Pages.Pessengers;

namespace Autostop.Client.Mobile.UI.IoC
{
    public static class MobileRegistryExtenstion
    {
	    public static void ClientTypesRegistry(this ContainerBuilder builder)
	    {
		    builder.RegisterType<DestinationSearchPlacePage>().As<IScreenFor<DestinationSearchPlaceViewModel>>();
		    builder.RegisterType<PickupSearchPlacePage>().As<IScreenFor<PickupSearchPlaceViewModel>>();
	    }
    }
}
