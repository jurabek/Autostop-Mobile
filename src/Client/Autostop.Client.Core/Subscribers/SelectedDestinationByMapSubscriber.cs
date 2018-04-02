using Autostop.Client.Abstraction.Publishers;
using Autostop.Client.Abstraction.Subscribers;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Core.Subscribers
{
    public class SelectedDestinationByMapSubscriber : ISelectedDestinationByMapSubscriber
	{
		public SelectedDestinationByMapSubscriber(ISelectedDestinationByMapPublisher publisher)
		{
			Publisher = publisher;
		}

		public IPublisher<Address> Publisher { get; }
	}
}
