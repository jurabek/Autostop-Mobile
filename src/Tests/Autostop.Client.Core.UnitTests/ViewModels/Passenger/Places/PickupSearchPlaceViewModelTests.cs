using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Core.Models;
using Autostop.Client.Core.ViewModels.Passenger.Places;
using Microsoft.Reactive.Testing;
using NUnit.Framework;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Autostop.Client.Core.Extensions;

namespace Autostop.Client.Core.UnitTests.ViewModels.Passenger.Places
{
    public class PickupSearchPlaceViewModelTests : BaseAutoMockedReactiveTest<PickupSearchPlaceViewModel>
    {
		[Test, AutoDomainData]
	    public void Given_empty_search_text_SearchResult_should_return_Home_and_Work_result_model(HomeResultModel homeResultModel, WorkResultModel workResultModel)
		{
			var viewModel = ClassUnderTest;
			var autoCompleteResultProviderMoq = GetMock<IEmptyAutocompleteResultProvider>();
			autoCompleteResultProviderMoq.Setup(x => x.GetHomeResultModel()).Returns(homeResultModel);
			autoCompleteResultProviderMoq.Setup(x => x.GetWorkResultModel()).Returns(workResultModel);

			// Act 
			//viewModel.ObservablePropertyChanged(() => viewModel.SearchText)
			//	.Throttle(TimeSpan.FromSeconds(300), Scheduler)
			//	.Subscribe(async searchText =>
			//	{
			//		await viewModel.Search(searchText);
			//	});

			Scheduler.Schedule(() => viewModel.SearchText = string.Empty);
			Scheduler.Start();
			var result = viewModel.SearchResults;

			// Assert
			//Assert.AreEqual(homeResultModel, result[0]);
			//Assert.AreEqual(workResultModel, result[1]);
		}

	    [Test]
	    public void Given_search_text_SearchResult_shuould_return_List_of_IAutoComplete_results()
	    {
		    
	    }
	}

}
