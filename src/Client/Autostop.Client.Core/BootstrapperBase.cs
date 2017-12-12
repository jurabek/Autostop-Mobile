using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
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

			ContainerRegistery(builder);

		    Container = builder.Build();
	    }


	    protected abstract void ContainerRegistery(ContainerBuilder builder);
    }
}
