using System;
using System.Collections.Generic;
using System.Text;
using Autostop.Client.Abstraction.Models;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Core.Models;

namespace Autostop.Client.Core.Providers
{
    public class EmptyAutocompleteResultProvider : IEmptyAutocompleteResultProvider
    {
	    private readonly ISettingsProvider _settingsProvider;

	    public EmptyAutocompleteResultProvider(ISettingsProvider settingsProvider)
	    {
		    _settingsProvider = settingsProvider;
	    }

	    public IAutoCompleteResultModel GetHomeResultModel()
	    {   
		    return new HomeResultModel(_settingsProvider.HomeAddress);
	    }

	    public IAutoCompleteResultModel GetWorkResultModel()
	    {
		    return new WorkResultModel(_settingsProvider.WorkAddress);
	    }

	    public IAutoCompleteResultModel GetSetLocationOnMapResultModel()
	    {
		    return new SetLocationOnMapResultModel();
	    }
	}
}
