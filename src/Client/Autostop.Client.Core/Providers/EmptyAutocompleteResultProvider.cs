using Autostop.Client.Abstraction.Models;
using Autostop.Client.Abstraction.Providers;
using Autostop.Client.Core.Models;
using JetBrains.Annotations;

namespace Autostop.Client.Core.Providers
{
    [UsedImplicitly]
    public class EmptyAutocompleteResultProvider : IEmptyAutocompleteResultProvider
    {
	    private readonly ISettingsProvider _settingsProvider;

	    public EmptyAutocompleteResultProvider(ISettingsProvider settingsProvider)
	    {
		    _settingsProvider = settingsProvider;
	    }

	    public IAutoCompleteResultModel GetHomeResultModel()
	    {   
		    return new HomeResultModel(_settingsProvider.GetHomeAddress());
	    }

	    public IAutoCompleteResultModel GetWorkResultModel()
	    {
		    return new WorkResultModel(_settingsProvider.GetWorkAddress());
	    }

	    public IAutoCompleteResultModel GetSetLocationOnMapResultModel()
	    {
		    return new SetLocationOnMapResultModel();
	    }
	}
}
