using Autostop.Client.Abstraction.Models;

namespace Autostop.Client.Abstraction.Providers
{
	public interface IEmptyAutocompleteResultProvider
	{
		IAutoCompleteResultModel GetHomeResultModel();
		IAutoCompleteResultModel GetSetLocationOnMapResultModel();
		IAutoCompleteResultModel GetWorkResultModel();
	}
}