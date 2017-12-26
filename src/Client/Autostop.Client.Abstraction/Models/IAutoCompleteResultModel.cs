namespace Autostop.Client.Abstraction.Models
{
    public interface IAutoCompleteResultModel
    {
        string PlaceId { get; set; }
        string PrimaryText { get; set; }
        string SecondaryText { get; set; }
        string Icon { get; set; }
    }
}