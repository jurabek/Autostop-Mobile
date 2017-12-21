namespace Autostop.Client.Abstraction
{
    public interface IScreenFor<TViewModel>
    {
        TViewModel ViewModel { get; set; }
    }
}