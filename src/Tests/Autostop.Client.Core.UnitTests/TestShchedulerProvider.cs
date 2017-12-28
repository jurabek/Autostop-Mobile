using System.Reactive.Concurrency;
using Autostop.Client.Abstraction.Providers;
using Microsoft.Reactive.Testing;

namespace Autostop.Client.Core.UnitTests
{
	public class TestShchedulerProvider : ISchedulerProvider
	{
		private readonly TestScheduler _scheduler;

		public TestShchedulerProvider(TestScheduler scheduler)
		{
			_scheduler = scheduler;
		}

		public IScheduler DefaultScheduler => _scheduler;

		public IScheduler SynchronizationContextScheduler => _scheduler;
	}
}