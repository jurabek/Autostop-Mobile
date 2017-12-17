namespace Autostop.Client.Abstraction.Adapters
{
	public interface IViewAdapter<out TView> where TView : class
	{
		TView GetView<TViewModel>(IScreenFor<TViewModel> view);
	}
}