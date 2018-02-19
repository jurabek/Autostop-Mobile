using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Autostop.Client.Abstraction.Models;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Abstraction.Services;
using Autostop.Client.Core.Models;
using Autostop.Client.Core.ViewModels.Passenger.ChooseOnMap;
using Autostop.Client.Core.ViewModels.Passenger.LocationEditor;
using Autostop.Common.Shared.Models;
using Moq;
using NUnit.Framework;

namespace Autostop.Client.Core.UnitTests.ViewModels.Passenger.LocationEditor
{
    public class HomeLocationEditorViewModelTests : BaseAutoMockedReactiveTest<HomeLocationEditorViewModel>
    {
        [Test, AutoDomainData]
        public void Search_ReturnsSetLocationOnMapResultModel_WhenTextSearchEmpty(SetLocationOnMapResultModel locationOnMapResultModel)
        {
            // Act 
            var viewModel = ClassUnderTest;
            var autoCompleteResultProviderMoq = GetMock<IEmptyAutocompleteResultProvider>();
            autoCompleteResultProviderMoq.Setup(x => x.GetSetLocationOnMapResultModel()).Returns(locationOnMapResultModel);

            // Arrange
            Scheduler.Schedule(() => viewModel.SearchText = string.Empty);
            Scheduler.Start();
            var result = viewModel.SearchResults;

            // Assert
            Assert.AreEqual(locationOnMapResultModel, result[0]);
        }

        [Test, AutoDomainData]
        public void SelectedSearchResult_NavigatesToChooseHomeAddressOnMapViewModel_WhenSelectedSearchResultIsSetLocationOnMapResultModel(SetLocationOnMapResultModel locationOnMapResultModel)
        {
            // Act 
            var viewModel = ClassUnderTest;
            var autoCompleteResultProviderMoq = GetMock<IEmptyAutocompleteResultProvider>();
            autoCompleteResultProviderMoq.Setup(x => x.GetSetLocationOnMapResultModel()).Returns(locationOnMapResultModel);

            // Arrange
            Scheduler.Schedule(() => viewModel.SearchText = string.Empty);
            Scheduler.Start();
            viewModel.SelectedSearchResult = viewModel.SearchResults[0];

            // Assert
            GetMock<INavigationService>().Verify(x => x.NavigateTo<ChooseHomeAddressOnMapViewModel>(), Times.Once);
        }

        [Test, AutoDomainData]
        public void SelectedSearchResult_SetsHomeAddress_WhenSelectedResultIsAutoCompleteResultModel(IEnumerable<AutoCompleteResultModel> autoCompleteResultModels, Address address)
        {
            // Act
            var viewModel = ClassUnderTest;
            string searchText = "Ashan, Dushanbe";
            var expectedModels = new ObservableCollection<IAutoCompleteResultModel>(autoCompleteResultModels);

            GetMock<IPlacesProvider>().Setup(x => x.GetAutoCompleteResponse(searchText))
                .Returns(Task.FromResult(expectedModels));

            GetMock<IGeocodingProvider>().Setup(x => x.ReverseGeocodingFromPlaceId(expectedModels[0].PlaceId))
                .Returns(Task.FromResult(address));

            // Arrange
            Scheduler.Schedule(() => viewModel.SearchText = searchText);
            Scheduler.Start();

            viewModel.SelectedSearchResult = viewModel.SearchResults[0];

            GetMock<ISettingsProvider>().Verify(x => x.SetHomeAddress(address), Times.Once());
            GetMock<INavigationService>().Verify(x => x.GoBack(), Times.Once);
        }

        [Test]
        public void PlaceHolderText_should_be_some_string()
        {
            Assert.That(ClassUnderTest.PlaceholderText, Is.Not.Null);
        }
    }
}
