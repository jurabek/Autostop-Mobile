using System.Reactive.Concurrency;

namespace Autostop.Client.Abstraction.Providers
{
	public interface ISchedulerProvider
	{
		IScheduler DefaultScheduler { get; }
		IScheduler SynchronizationContextScheduler { get; }
	}
}