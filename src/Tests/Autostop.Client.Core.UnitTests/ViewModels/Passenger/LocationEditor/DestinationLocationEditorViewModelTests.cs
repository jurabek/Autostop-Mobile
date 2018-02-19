using System;
using System.Collections.ObjectModel;
using System.Reactive.Concurrency;
using AutoFixture;
using Autostop.Client.Abstraction.Factories;
using Autostop.Client.Abstraction.Models;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Core.Models;
using Autostop.Client.Core.ViewModels.Passenger.ChooseOnMap;
using Autostop.Client.Core.ViewModels.Passenger.LocationEditor;
using Moq;
using NUnit.Framework;

namespace Autostop.Client.Core.UnitTests.ViewModels.Passenger.LocationEditor
{
    public class DestinationLocationEditorViewModelTests : BaseAutoMockedReactiveTest<DestinationLocationEditorViewModel>
    {
	    [Test, AutoDomainData]
	    public void Search_ReturnsHomeAndWorkResultModel_WhenTextSearchIsEmpty(HomeResultModel homeResultModel, WorkResultModel workResultModel, SetLocationOnMapResultModel setLocationOnMapResultModel)
	    {
		    // Act 
		    var viewModel = ClassUnderTest;
		    var autoCompleteResultProviderMoq = GetMock<IEmptyAutocompleteResultProvider>();
		    autoCompleteResultProviderMoq.Setup(x => x.GetHomeResultModel()).Returns(homeResultModel);
		    autoCompleteResultProviderMoq.Setup(x => x.GetWorkResultModel()).Returns(workResultModel);
		    autoCompleteResultProviderMoq.Setup(x => x.GetSetLocationOnMapResultModel()).Returns(setLocationOnMapResultModel);

			// Arrange
			Scheduler.Schedule(() => viewModel.SearchText = string.Empty);
		    Scheduler.Start();
		    var result = viewModel.SearchResults;

		    // Assert
		    Assert.AreEqual(homeResultModel, result[0]);
		    Assert.AreEqual(workResultModel, result[1]);
		    Assert.AreEqual(setLocationOnMapResultModel, result[2]);
		}

		[Test, AutoDomainData]
		public void SelectedSearchResult_NavigatesToChoseDestinationOnMapViewModel_WhenResultViewModelEqualToSetLocationOnMapResultModel(Fixture fixture)
		{
			// Arrange
			var viewModel = ClassUnderTest;
			var choseDetinationViewModel = Mocker.Create<ChooseDestinationOnMapViewModel>();
			var setLocationOnMapResultModel = fixture.Create<SetLocationOnMapResultModel>();
			var homeResultModel = fixture.Create<HomeResultModel>();
			var workResultModel = fixture.Create<WorkResultModel>();
			viewModel.SearchResults = new ObservableCollection<IAutoCompleteResultModel>
			{
				setLocationOnMapResultModel,
				homeResultModel,
				workResultModel
			};

			GetMock<IChooseOnMapViewModelFactory>().Setup(x => x.GetChooseDestinationOnMapViewModel())
				.Returns(choseDetinationViewModel);

			// Act
		    viewModel.Load();
			Scheduler.Schedule(() => viewModel.SelectedSearchResult = viewModel.SearchResults[0]);
			Scheduler.Start();

			// Assert
			GetMock<INavigationService>().Verify(x => x.NavigateTo(choseDetinationViewModel), Times.Once);
		}

		[Test]
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
		    // Arrange
		    Scheduler.Schedule(() => viewModel.SelectedSearchResult = viewModel.SearchResults[0]);
		    Scheduler.Schedule(() => viewModel.SelectedSearchResult = viewModel.SearchResults[1]);
		    Scheduler.Start();

		    // Assert
		    navigationServiceMoq.Verify(x => x.NavigateToSearchView(It.IsAny<Action<HomeLocationEditorViewModel>>()), Times.Once);
		    navigationServiceMoq.Verify(x => x.NavigateToSearchView(It.IsAny<Action<WorkLocationEditorViewModel>>()), Times.Once);
	    }
	}
}
