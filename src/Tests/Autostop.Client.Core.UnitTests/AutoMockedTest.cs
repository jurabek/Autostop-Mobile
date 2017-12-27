
using System.Reactive.Concurrency;
using System.Threading;
using Autofac.Extras.Moq;
using Microsoft.Reactive.Testing;
using Moq;
using NUnit.Framework;

namespace Autostop.Client.Core.UnitTests
{
    public abstract class BaseAutoMockedTest<T>
        where T : class
    {
        protected virtual T ClassUnderTest => Mocker.Create<T>();

        protected AutoMock Mocker { get; private set; }

        [SetUp]
        public virtual void Init()
        {
            Mocker = AutoMock.GetLoose();
        }

        [TearDown]
        public void TearDown()
        {
            Mocker?.Dispose();
        }

        protected Mock<TDepend> GetMock<TDepend>()
            where TDepend : class
        {
            return Mocker.Mock<TDepend>();
        }
    }

	public abstract class BaseAutoMockedReactiveTest<T>
		where T : class
	{
		protected virtual T ClassUnderTest => Mocker.Create<T>();

		protected readonly TestScheduler Scheduler = new TestScheduler();

		protected AutoMock Mocker { get; private set; }

		[SetUp]
		public virtual void Init()
		{
			Mocker = AutoMock.GetLoose();
			Mocker.Provide<IScheduler>(Scheduler);
			SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
		}

		[TearDown]
		public void TearDown()
		{
			Mocker?.Dispose();
		}

		protected Mock<TDepend> GetMock<TDepend>()
			where TDepend : class
		{
			return Mocker.Mock<TDepend>();
		}
	}

}