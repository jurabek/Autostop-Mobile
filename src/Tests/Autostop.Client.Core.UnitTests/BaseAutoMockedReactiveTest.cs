using System.Threading;
using Autofac.Extras.Moq;
using Autostop.Client.Abstraction.Providers;
using Microsoft.Reactive.Testing;
using Moq;
using NUnit.Framework;

namespace Autostop.Client.Core.UnitTests
{
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
			Mocker.Provide<ISchedulerProvider>(new TestShchedulerProvider(Scheduler));
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