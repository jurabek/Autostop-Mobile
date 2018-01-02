using System.Reactive.Concurrency;
using System.Threading;
using Autostop.Client.Abstraction.Providers;
using JetBrains.Annotations;

namespace Autostop.Client.Core.Providers
{
    [UsedImplicitly]
    public class SchedulerProvider : ISchedulerProvider
	{
	    public IScheduler DefaultScheduler => Scheduler.Default;

	    public IScheduler SynchronizationContextScheduler => new SynchronizationContextScheduler(SynchronizationContext.Current);
    }
}
