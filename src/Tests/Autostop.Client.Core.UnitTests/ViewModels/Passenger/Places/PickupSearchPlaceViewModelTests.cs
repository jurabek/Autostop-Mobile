using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Core.Models;
using Autostop.Client.Core.ViewModels.Passenger.Places;
using NUnit.Framework;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using AutoFixture;
using Autostop.Client.Abstraction.Models;
using Autostop.Client.Abstraction.Services;
using Autostop.Common.Shared.Models;
using Moq;

namespace Autostop.Client.Core.UnitTests.ViewModels.Passenger.Places
{
	public class PickupSearchPlaceViewModelTests : BaseAutoMockedReactiveTest<PickupSearchPlaceViewModel>
	{
		[Test, AutoDomainData]
		public void Search_ReturnsHomeAndWorkResultModel_WhenTextSearchIsEmpty(HomeResultModel homeResultModel, WorkResultModel workResultModel)
		{
			// Act 
			var viewModel = ClassUnderTest;
			var autoCompleteResultProviderMoq = GetMock<IEmptyAutocompleteResultProvider>();
			autoCompleteResultProviderMoq.Setup(x => x.GetHomeResultModel()).Returns(homeResultModel);
			autoCompleteResultProviderMoq.Setup(x => x.GetWorkResultModel()).Returns(workResultModel);

			// Arrange
			Scheduler.Schedule(() => viewModel.SearchText = string.Empty);
			Scheduler.Start();
			var result = viewModel.SearchResults;

			// Assert
			Assert.AreEqual(homeResultModel, result[0]);
			Assert.AreEqual(workResultModel, result[1]);
		}

		[Test, AutoDomainData]
		public void Search_ReturnsAutoCompleteResults_WhenSearchTextIsValid(IEnumerable<AutoCompleteResultModel> autoCompleteResultModels)
		{
			// Act
			var viewModel = ClassUnderTest;
			string searchText = "Ashan, Dushanbe";
			var expectedModels = new ObservableCollection<IAutoCompleteResultModel>(autoCompleteResultModels);

			GetMock<IPlacesProvider>().Setup(x => x.GetAutoCompleteResponse(searchText))
				.Returns(Task.FromResult(expectedModels));

			// Arrange
			Scheduler.Schedule(() => viewModel.SearchText = searchText);
			Scheduler.Start();
			var result = viewModel.SearchResults;

			Assert.That(result, Is.EqualTo(expectedModels));
		}

		[Test, AutoDomainData]
		public void SelectedSearchResult_NavigatesToAddressViewModel_WhenResultModelsAddressIsNull()
		{
			// Act
			var viewModel = ClassUnderTest;
			var homeResultModel = new HomeResultModel(null);
			var workResultModel = new WorkResultModel(null);
			viewModel.SearchResults = new ObservableCollection<IAutoCompleteResultModel>
			{
				homeResultModel,
				workResultModel
			};
			var navigationServiceMoq = GetMock<INavigationService>();

			var searchHomeViewModel = Mocker.Create<SearchHomeAddressViewModel>();
			var searchWorkViewModel = Mocker.Create<SearchWorkAddressViewModel>();

			navigationServiceMoq.Setup(x => x.NavigateToSearchView(It.IsAny<Action<SearchHomeAddressViewModel>>()))
				.Callback<Action<SearchHomeAddressViewModel>>(model =>
				{
					model(searchHomeViewModel);
				});

			navigationServiceMoq.Setup(x => x.NavigateToSearchView(It.IsAny<Action<SearchWorkAddressViewModel>>()))
				.Callback<Action<SearchWorkAddressViewModel>>(model =>
				{
					model(searchWorkViewModel);
				});

			// Arrange
			Scheduler.Schedule(() => viewModel.SelectedSearchResult = viewModel.SearchResults[0]);
			Scheduler.Schedule(() => viewModel.SelectedSearchResult = viewModel.SearchResults[1]);
			Scheduler.Start();

			// Assert
			navigationServiceMoq.Verify(x => x.NavigateToSearchView(It.IsAny<Action<SearchHomeAddressViewModel>>()), Times.Once);
			navigationServiceMoq.Verify(x => x.NavigateToSearchView(It.IsAny<Action<SearchWorkAddressViewModel>>()), Times.Once);
		}

		[Test, AutoDomainData]
		public void SelectedSearchResult_RaisesSelectedAddressObservable_WhenHomeResultModelHasAddress(Fixture fixture)
		{
			// Act
			Address expectedAddress = null;
			var viewModel = ClassUnderTest;
			var homeResultModel = fixture.Create<HomeResultModel>();
			var workResultModel = fixture.Create<WorkResultModel>();
			viewModel.SearchResults = new ObservableCollection<IAutoCompleteResultModel>
			{
				homeResultModel,
				workResultModel
			};
			viewModel.SelectedAddress.Subscribe(address => expectedAddress = address);

			// Arrange
			Scheduler.Schedule(() => viewModel.SelectedSearchResult = viewModel.SearchResults[0]);
			Scheduler.Start();

			// Assert
			Assert.That(homeResultModel.Address, Is.EqualTo(expectedAddress));
		}

		[Test, AutoDomainData]
		public void SelectedSearchResult_RaisesSelectedAddressObservable_WhenWorkAddressModelHasAddress(Fixture fixture)
		{
			// Act
			Address expectedAddress = null;
			var viewModel = ClassUnderTest;
			var homeResultModel = fixture.Create<HomeResultModel>();
			var workResultModel = fixture.Create<WorkResultModel>();
			viewModel.SearchResults = new ObservableCollection<IAutoCompleteResultModel>
			{
				homeResultModel,
				workResultModel
			};
			viewModel.SelectedAddress.Subscribe(address => expectedAddress = address);

			// Arrange
			Scheduler.Schedule(() => viewModel.SelectedSearchResult = viewModel.SearchResults[1]);
			Scheduler.Start();

			// Assert
			Assert.That(workResultModel.Address, Is.EqualTo(expectedAddress));
		}

		[Test, AutoDomainData]
		public void SelectedSearchResult_ReversesAddressFromPlaceIdAndRaisesSelectedAddressObservable_WhenAutoCompleteResultModel(Fixture fixture, IEnumerable<AutoCompleteResultModel> autoCompleteResultModels, Address geocodingAddress)
		{
			// Act
			Address expectedAddress = null;
			var viewModel = ClassUnderTest;
			viewModel.SearchResults = new ObservableCollection<IAutoCompleteResultModel>(autoCompleteResultModels);
			var selectedSearchResult = viewModel.SearchResults[0];
			GetMock<IGeocodingProvider>().Setup(x => x.ReverseGeocodingFromPlaceId(selectedSearchResult.PlaceId)).Returns(Task.FromResult(geocodingAddress));
			viewModel.SelectedAddress.Subscribe(a => expectedAddress = a);

			// Arrange
			Scheduler.Schedule(() => viewModel.SelectedSearchResult = selectedSearchResult);
			Scheduler.Start();

			//  Assert
			Assert.That(geocodingAddress, Is.EqualTo(expectedAddress));
		}

		[Test]
		public void GoBackCommand_NavigatesToBack_WhenExecuted()
		{
			ClassUnderTest.GoBack.Execute(null);

			GetMock<INavigationService>().Verify(x => x.GoBack(), Times.Once);
		}

		[Test]
		public void PlaceHolderText_should_be_some_string()
		{
			Assert.That(ClassUnderTest.PlaceholderText, Is.Not.Null);
		}

		[Test]
		public void IsSearching_Set_and_Get()
		{
			var viewModel = ClassUnderTest;
			viewModel.IsSearching = true;

			Assert.True(viewModel.IsSearching);
		}
	}
}
