using Android.Gms.Maps.Model;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Android.Abstraction
{
	public interface IMarkerAdapter
	{
		MarkerOptions GetMarkerOptions(DriverLocation driverLocation);
	}
}