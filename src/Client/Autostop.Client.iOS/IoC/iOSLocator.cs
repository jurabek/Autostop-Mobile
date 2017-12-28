using Autofac;
using Autostop.Client.Abstraction;
using Autostop.Client.Abstraction.Adapters;
using Autostop.Client.Abstraction.Managers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Core.IoC;
using Autostop.Client.Core.ViewModels.Passenger;
using Autostop.Client.iOS.Adapters;
using Autostop.Client.iOS.Managers;
using Autostop.Client.iOS.Services;
using Autostop.Client.iOS.Views.Passenger;
using Autostop.Client.Mobile.UI.IoC;
using UIKit;

namespace Autostop.Client.iOS.IoC
{
    // ReSharper disable once InconsistentNaming
    public class iOSLocator : Locator
    {
        protected override void ContainerRegistery(ContainerBuilder builder)
        {
            builder.RegisterType<LocationManager>().As<ILocationManager>().SingleInstance();
            builder.RegisterType<PageToViewControllerAdapter>().As<IViewAdapter<UIViewController>>();
            builder.RegisterType<NavigationService>().As<INavigationService>();
            builder.RegisterType<MainViewController>().As<IScreenFor<MainViewModel>>();
	        builder.RegisterType<ChooseDestinationOnMapViewController>().As<IScreenFor<ChooseDestinationOnMapViewModel>>();
	        builder.RegisterType<ChooseHomeAddressOnMapViewController>().As<IScreenFor<ChooseHomeAddressOnMapViewModel>>();
	        builder.RegisterType<ChooseWorkAddressOnMapViewController>().As<IScreenFor<ChooseWorkAddressOnMapViewModel>>();

            builder.ClientTypesRegistry();
        }
    }
}