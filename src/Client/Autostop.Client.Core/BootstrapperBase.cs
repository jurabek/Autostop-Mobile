using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Autostop.Client.Abstraction.Adapters;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Core.Constants;
using Autostop.Client.Core.Providers;
using Autostop.Client.Core.ViewModels.Passenger;
using Google.Maps;
using Google.Maps.Geocoding;
using Google.Maps.Places;

namespace Autostop.Client.Core
{
    public abstract class BootstrapperBase
    {
	    public static IContainer Container;

	    public IContainer Build()
	    {
			var builder = new ContainerBuilder();
		    var googleSigned = new GoogleSigned(GoogleMapsApi.ClientApiKey);

			builder.RegisterType<MainViewModel>().AsSelf();
		    builder.RegisterType<PickupSearchPlaceViewModel>().AsSelf();
		    builder.RegisterType<DestinationSearchPlaceViewModel>().AsSelf();
			builder.RegisterType<GeocodingProvider>().As<IGeocodingProvider>();
		    builder.RegisterType<PlacesProvider>().As<IPlacesProvider>();
		    builder.RegisterInstance(new PlacesService(googleSigned)).As<IPlacesService>();
		    builder.RegisterInstance(new GeocodingService(googleSigned)).As<IGeocodingService>();

			ContainerRegistery(builder);

		    Container = builder.Build();
	        return Container;
	    }


	    protected abstract void ContainerRegistery(ContainerBuilder builder);
    }
}
