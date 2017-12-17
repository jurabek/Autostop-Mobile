using Autostop.Client.Abstraction;
using Autostop.Client.Abstraction.Adapters;
using UIKit;
using Xamarin.Forms;

namespace Autostop.Client.iOS.Adapters
{
	public class PageToViewControllerAdapter : IViewAdapter<UIViewController>
	{
		public UIViewController GetView<TViewModel>(IScreenFor<TViewModel> view)
		{
			switch (view)
			{
				case Page page:
					return page.CreateViewController();
				case UIViewController resultView:
					return resultView;
			}

			return default(UIViewController);
		}
	}
}
