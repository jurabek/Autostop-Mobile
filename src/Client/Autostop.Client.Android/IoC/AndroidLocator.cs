using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Autofac;
using Autostop.Client.Core.IoC;

namespace Autostop.Client.Android.IoC
{
	public class AndroidLocator : Locator
	{
		protected override void ContainerRegistery(ContainerBuilder builder)
		{
			throw new NotImplementedException();
		}
	}
}