using Autostop.Client.Abstraction.Adapters;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Core.Adapters
{
	public class LocationAdapter : ILocationAdapter
	{
		public Location GetLocationFromCoordinate(double latitude, double longitude)
		{
			return new Location(latitude, longitude);
		}
	}
}
