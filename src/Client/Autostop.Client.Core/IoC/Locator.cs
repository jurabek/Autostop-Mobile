using System;
using System.Reactive.Concurrency;
using Autofac;
using Autofac.Core;
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
using ViewModelKeys = Autostop.Common.Shared.Constants.IoC.ViewModelKeys;

namespace Autostop.Client.Core.IoC
{
    public abstract class Locator
    {
        private static IContainer _container;

        public IContainer Build()
        {
            var builder = new ContainerBuilder();
            var googleSigned = new GoogleSigned(GoogleMapsApi.ClientApiKey);

	        builder.RegisterType<RideViewModel>().As<IRideViewModel>();
            builder.RegisterType<MainViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<PickupSearchPlaceViewModel>().Named<IBaseSearchPlaceViewModel>(ViewModelKeys.PickupSearch);
            builder.RegisterType<DestinationSearchPlaceViewModel>().Named<IBaseSearchPlaceViewModel>(ViewModelKeys.DestinationSearch);
            builder.RegisterType<ChooseDestinationOnMapViewModel>().Named<ISearchableViewModel>(ViewModelKeys.ChooseDestinationOnMap);

            builder.RegisterType<SearchHomeAddressViewModel>().AsSelf();
	        builder.RegisterType<SearchWorkAddressViewModel>().AsSelf();
	        builder.RegisterType<ChooseHomeAddressOnMapViewModel>().AsSelf();
	        builder.RegisterType<ChooseWorkAddressOnMapViewModel>().AsSelf();

			builder.RegisterType<GeocodingProvider>().As<IGeocodingProvider>();
            builder.RegisterType<PlacesProvider>().As<IPlacesProvider>();
	        builder.RegisterType<EmptyAutocompleteResultProvider>().As<IEmptyAutocompleteResultProvider>();
	        builder.RegisterType<SchedulerProvider>().As<ISchedulerProvider>();
            builder.RegisterInstance(new PlacesService(googleSigned)).As<IPlacesService>();
            builder.RegisterInstance(new GeocodingService(googleSigned)).As<IGeocodingService>();

            builder.RegisterType<ViewFactory>().As<IViewFactory>();
            builder.RegisterType<SearchPlaceViewModelFactory>().As<ISearchPlaceViewModelFactory>();
            builder.RegisterType<ChooseOnMapViewModelFactory>().As<IChooseOnMapViewModelFactory>();
            builder.RegisterType<AutoMapperFacade>().As<IAutoMapperFacade>();
	        builder.RegisterInstance(Scheduler.Default).As<IScheduler>();

            ContainerRegistery(builder);
            _container = builder.Build();

            return _container;
        }

	    public static TService ResolveNamed<TService>(string serviceName)
	    {
		    return _container.ResolveNamed<TService>(serviceName);
	    }

		public static TService ResolveNamed<TService>(string serviceName, params Parameter[] parameters)
	    {
		    return _container.ResolveNamed<TService>(serviceName, parameters);
	    }

	    public static TService Resolve<TService>()
	    {
		    return _container.Resolve<TService>();
	    }

	    public static object Resolve(Type serviceType)
	    {
		    return _container.Resolve(serviceType);
		}

		protected abstract void ContainerRegistery(ContainerBuilder builder);
    }
}