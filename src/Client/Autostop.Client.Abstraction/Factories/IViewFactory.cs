namespace Autostop.Client.Abstraction.Factories
{
    /// <summary>
    ///   Abstaction for for creating views from view model first implementation
    /// </summary>
    public interface IViewFactory
    {
        IScreenFor<TViewModel> CreateView<TViewModel>(TViewModel vm);
    }
}