using System.Reactive.Concurrency;
using Autofac;
using Autostop.Client.Abstraction.Facades;
using Autostop.Client.Abstraction.Factories;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Abstraction.ViewModels.Passenger.Places;
using Autostop.Client.Core.Constants;
using Autostop.Client.Core.Facades;
using Autostop.Client.Core.Factories;
using Autostop.Client.Core.Providers;
using Autostop.Client.Core.ViewModels.Passenger;
using Autostop.Client.Core.ViewModels.Passenger.Places;
using Google.Maps;
using Google.Maps.Geocoding;
using Google.Maps.Places;
using Const = Autostop.Common.Shared.Constants.IoC;

namespace Autostop.Client.Core.IoC
{
    public abstract class Locator
    {
        public static IContainer Container;

        public IContainer Build()
        {
            var builder = new ContainerBuilder();
            var googleSigned = new GoogleSigned(GoogleMapsApi.ClientApiKey);

	        builder.RegisterType<RideViewModel>().As<IRideViewModel>();
            builder.RegisterType<MainViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<PickupSearchPlaceViewModel>().Named<IBaseSearchPlaceViewModel>(Const.ViewModelKeys.PickupSearch);
            builder.RegisterType<DestinationSearchPlaceViewModel>().Named<IBaseSearchPlaceViewModel>(Const.ViewModelKeys.DestinationSearch);
            builder.RegisterType<ChooseDestinationOnMapViewModel>().Named<ISearchableViewModel>(Const.ViewModelKeys.ChooseDestinationOnMap);

            builder.RegisterType<SearchHomeAddressViewModel>().AsSelf();
	        builder.RegisterType<SearchWorkAddressViewModel>().AsSelf();
	        builder.RegisterType<ChooseHomeAddressOnMapViewModel>().AsSelf();
	        builder.RegisterType<ChooseWorkAddressOnMapViewModel>().AsSelf();

			builder.RegisterType<GeocodingProvider>().As<IGeocodingProvider>();
            builder.RegisterType<PlacesProvider>().As<IPlacesProvider>();
	        builder.RegisterType<EmptyAutocompleteResultProvider>().As<IEmptyAutocompleteResultProvider>();
            builder.RegisterInstance(new PlacesService(googleSigned)).As<IPlacesService>();
            builder.RegisterInstance(new GeocodingService(googleSigned)).As<IGeocodingService>();

            builder.RegisterType<ViewFactory>().As<IViewFactory>();
            builder.RegisterType<SearchPlaceViewModelFactory>().As<ISearchPlaceViewModelFactory>();
            builder.RegisterType<ChooseOnMapViewModelFactory>().As<IChooseOnMapViewModelFactory>();
            builder.RegisterType<AutoMapperFacade>().As<IAutoMapperFacade>();
	        builder.RegisterInstance(Scheduler.Default).As<IScheduler>();

            ContainerRegistery(builder);
            Container = builder.Build();

            return Container;
        }

        protected abstract void ContainerRegistery(ContainerBuilder builder);
    }
}