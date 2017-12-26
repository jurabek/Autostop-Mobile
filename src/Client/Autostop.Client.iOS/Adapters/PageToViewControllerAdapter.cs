using Autostop.Client.Abstraction;
using Autostop.Client.Abstraction.Adapters;
using Autostop.Client.Abstraction.ViewModels.Passenger.Places;
using Autostop.Client.iOS.Extensions;
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

	    public UIViewController GetSearchView<TViewModel>(IScreenFor<TViewModel> view) where TViewModel : IBaseSearchPlaceViewModel
		{
		    if (view is Page page)
		    {
			    var navigationBar = ((UINavigationController) UIApplication.SharedApplication.KeyWindow.RootViewController).NavigationBar;

				var uiViewController = page.CreateViewController();
			    navigationBar.BarTintColor = UIColor.FromRGB(245, 245, 245);
			    uiViewController.CreateSearchViewOnNavigationBar(view.ViewModel);
				return uiViewController;
			}
			return null;
	    }
    }
}