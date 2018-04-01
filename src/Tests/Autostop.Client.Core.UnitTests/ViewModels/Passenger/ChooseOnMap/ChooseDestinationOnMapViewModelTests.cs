using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Core.ViewModels.Passenger.ChooseOnMap;
using Autostop.Common.Shared.Models;
using NUnit.Framework;
using System;
using Autostop.Client.Abstraction.Services;
using Moq;

namespace Autostop.Client.Core.UnitTests.ViewModels.Passenger.ChooseOnMap
{
    public class ChooseDestinationOnMapViewModelTests : BaseAutoMockedReactiveTest<ChooseDestinationOnMapViewModel>
    {
        [Test]
        public async Task IsSearching_ShouldBeTrue_WhenCameraStartMoving()
        {
            // Act
            var viewModel = ClassUnderTest;
            var camerStartMoving = new Subject<bool>();
            viewModel.CameraStartMoving = camerStartMoving;
            viewModel.CameraPositionChanged = new Subject<Location>();

            // Arrange
            await viewModel.Load();
            Scheduler.Schedule(() => camerStartMoving.OnNext(true));
            Scheduler.Start();

            // Assert
            Assert.True(viewModel.IsSearching);
        }

        [Test, AutoDomainData]
        public async Task Done_ShouldObserSelectedAddressAndNavigateToRoot_WhenCameraPositionChangedAndHasAddress(Address address, Location location)
        {
			// 
			var viewModel = ClassUnderTest;
			Address selectedAddress = null;
            var cameraPositionChanged = new Subject<Location>();
            viewModel.CameraStartMoving = new Subject<bool>();
            viewModel.CameraPositionChanged = cameraPositionChanged;
            GetMock<IGeocodingProvider>().Setup(x => x.ReverseGeocodingFromLocation(location)).Returns(Task.FromResult(address));

            // Arrange
            await viewModel.Load();
            Scheduler.Schedule(() => cameraPositionChanged.OnNext(location));
            Scheduler.Start();
            viewModel.Done.Execute(null);

            // Assert
            Assert.That(viewModel.SearchText, Is.EqualTo(address.FormattedAddress));
            GetMock<INavigationService>().Verify(x => x.NavigateToRoot(), Times.Once);
        }

		[Test]
	    public void GoBack_ShouldNavigateBack_WhenCommandExecutes()
	    {
			// Act

			// Arrange
			ClassUnderTest.GoBack.Execute(null);

			// Assert
			GetMock<INavigationService>().Verify(x => x.GoBack(), Times.Once);
	    }
    }
}
