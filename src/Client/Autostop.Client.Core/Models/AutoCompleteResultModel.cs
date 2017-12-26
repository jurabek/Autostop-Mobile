using Autostop.Client.Abstraction.Models;

namespace Autostop.Client.Core.Models
{
    public class AutoCompleteResultModel : IAutoCompleteResultModel
    {
        public string PrimaryText { get; set; }
        public string SecondaryText { get; set; }
        public string PlaceId { get; set; }
        public string Icon { get; set; }
    }
}