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

namespace Autostop.Client.Android.Exceptions
{
	internal class GooglePlayServicesException : Exception
	{
		public GooglePlayServicesException(string message) : base(message)
		{	
		}
	}
}