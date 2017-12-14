using System.Threading.Tasks;
using Autostop.Client.Abstraction.ViewModels.Passenger;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Abstraction.Providers
{
	public interface IGeocodingProvider
	{
		Task<string> ReverseGeocoding(Coordinate coordinate);
	}
}