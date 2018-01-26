using System;
using Autofac;
using Autofac.Core;
using Autostop.Client.Abstraction.Facades;
using Autostop.Client.Abstraction.Factories;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Core.Constants;
using Autostop.Client.Core.Facades;
using Autostop.Client.Core.Factories;
using Autostop.Client.Core.Providers;
using Autostop.Client.Core.ViewModels.Passenger;
using Autostop.Client.Core.ViewModels.Passenger.ChooseOnMap;
using Autostop.Client.Core.ViewModels.Passenger.LocationEditor;
using Autostop.Client.Core.ViewModels.Passenger.Trip;
using Autostop.Client.Core.ViewModels.Passenger.Welcome;
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

	        builder.RegisterType<TripLocationViewModel>().As<ITripLocationViewModel>();
            builder.RegisterType<MainViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<PickupLocationEditorViewModel>().Named<IBaseSearchPlaceViewModel>(ViewModelKeys.PickupSearch);
            builder.RegisterType<DestinationLocationEditorViewModel>().Named<IBaseSearchPlaceViewModel>(ViewModelKeys.DestinationSearch);
            builder.RegisterType<ChooseDestinationOnMapViewModel>().Named<ISearchableViewModel>(ViewModelKeys.ChooseDestinationOnMap);

            builder.RegisterType<HomeLocationEditorViewModel>().AsSelf();
	        builder.RegisterType<WorkLocationEditorViewModel>().AsSelf();
	        builder.RegisterType<ChooseHomeAddressOnMapViewModel>().AsSelf();
	        builder.RegisterType<ChooseWorkAddressOnMapViewModel>().AsSelf();
            builder.RegisterType<PhoneVerificationViewModel>().AsSelf();
			builder.RegisterType<SignInViewModel>().AsSelf();

			builder.RegisterType<GeocodingProvider>().As<IGeocodingProvider>();
            builder.RegisterType<PlacesProvider>().As<IPlacesProvider>();
	        builder.RegisterType<EmptyAutocompleteResultProvider>().As<IEmptyAutocompleteResultProvider>();
	        builder.RegisterType<SchedulerProvider>().As<ISchedulerProvider>();
	        builder.RegisterType<VisibleRegionProvider>().As<IVisibleRegionProvider>();
            builder.RegisterInstance(new PlacesService(googleSigned)).As<IPlacesService>();
            builder.RegisterInstance(new GeocodingService(googleSigned)).As<IGeocodingService>();

            builder.RegisterType<ViewFactory>().As<IViewFactory>();
            builder.RegisterType<SearchPlaceViewModelFactory>().As<ISearchPlaceViewModelFactory>();
            builder.RegisterType<ChooseOnMapViewModelFactory>().As<IChooseOnMapViewModelFactory>();
            builder.RegisterType<AutoMapperFacade>().As<IAutoMapperFacade>();

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