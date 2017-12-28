using Autostop.Client.Abstraction.Models;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Core.Models
{
    public class EmptyAutocompleteResultModel : IAutoCompleteResultModel
    {
	    public Address Address { get; set; }
		public string PlaceId { get; set; }
	    public string PrimaryText { get; set; }
	    public string SecondaryText { get; set; }
	    public string Icon { get; set; }
    }
}
