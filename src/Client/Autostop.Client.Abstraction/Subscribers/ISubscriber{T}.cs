using Autostop.Client.Abstraction.Publishers;

namespace Autostop.Client.Abstraction.Subscribers
{
    public interface ISubscriber<T>
    {
		IPublisher<T> Publisher { get; }
    }
}
