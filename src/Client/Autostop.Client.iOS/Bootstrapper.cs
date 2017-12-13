using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autostop.Client.Abstraction.Managers;
using Autostop.Client.Core;
using Autostop.Client.Core.ViewModels.Passenger;
using Autostop.Client.iOS.Managers;
using Foundation;
using GalaSoft.MvvmLight.Command;
using UIKit;

namespace Autostop.Client.iOS
{
	public class Bootstrapper : BootstrapperBase
	{
		protected override void ContainerRegistery(ContainerBuilder builder)
		{
			builder.RegisterType<LocationManager>().As<ILocationManager>().SingleInstance();
		}
	}
}