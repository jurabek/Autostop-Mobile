using System;

namespace Autostop.Client.Abstraction.Publishers
{
	public interface IPublisher<T>
	{
		IObservable<T> Handler { get; }

		void Publish(T data);
	}
}
