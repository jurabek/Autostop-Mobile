using Autostop.Client.Abstraction.ViewModels;

namespace Autostop.Client.Core.ViewModels
{
    public class AutoCompleteResultViewModel : BaseViewModel, IAutoCompleteResultViewModel
    {
        public string PrimaryText { get; set; }

        public string SecondaryText { get; set; }

        public string PlaceId { get; set; }

        public string Icon { get; set; }
    }
}