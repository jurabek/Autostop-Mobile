using System;
using System.Reactive.Subjects;
using Autostop.Client.Abstraction.Publishers;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Core.Publishers
{
    public class SelectedDestinationByMapPublisher : ISelectedDestinationByMapPublisher
	{
		readonly Subject<Address> _handler = new  Subject<Address>();

		public IObservable<Address> Handler => _handler;

		public void Publish(Address data)
		{
			_handler.OnNext(data);
		}
	}
}
