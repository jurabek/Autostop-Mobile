using Android.Gms.Maps.Model;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Android.Platform.Android.Abstraction
{
	public interface IMarkerAdapter
	{
		MarkerOptions GetMarkerOptions(DriverLocation driverLocation);
	}
}