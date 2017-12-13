using Autostop.Client.Abstraction.Adapters;
using Autostop.Common.Shared.Models;
using JetBrains.Annotations;

namespace Autostop.Client.Core.Adapters
{
	[UsedImplicitly]
	public class LocationAdapter : ILocationAdapter
	{
		public Location GetLocationFromCoordinate(double latitude, double longitude)
		{
			return new Location(latitude, longitude);
		}
	}
}
