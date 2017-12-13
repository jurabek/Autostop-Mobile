using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Autostop.Client.Abstraction.Adapters;
using Autostop.Client.Core.Adapters;
using Autostop.Client.Core.ViewModels.Passenger;

namespace Autostop.Client.Core
{
    public abstract class BootstrapperBase
    {
	    public static IContainer Container;

	    public void Build()
	    {
			var builder = new ContainerBuilder();

		    builder.RegisterType<MainViewModel>().AsSelf();
	        builder.RegisterType<LocationAdapter>().As<ILocationAdapter>();

			ContainerRegistery(builder);

		    Container = builder.Build();
	    }


	    protected abstract void ContainerRegistery(ContainerBuilder builder);
    }
}
