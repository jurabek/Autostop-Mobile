namespace Autostop.Client.Abstraction.ViewModels
{
    public interface IAutoCompleteResultViewModel
    {
        string PlaceId { get; set; }
        string PrimaryText { get; set; }
        string SecondaryText { get; set; }
        string Icon { get; set; }
    }
}