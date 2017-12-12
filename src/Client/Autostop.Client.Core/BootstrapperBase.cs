using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace Autostop.Client.Core
{
    public abstract class BootstrapperBase
    {
	    public static IContainer Container;

	    public void Build()
	    {
			var builder = new ContainerBuilder();

			ContainerRegistery(builder);
	    }


	    protected abstract void ContainerRegistery(ContainerBuilder builder);
    }
}
