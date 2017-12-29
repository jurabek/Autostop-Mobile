using System;
using System.Diagnostics;
using Autostop.Client.Abstraction;
using Autostop.Client.Abstraction.Adapters;
using Autostop.Client.Abstraction.ViewModels.Passenger.Places;
using Autostop.Client.iOS.Extensions;
using JetBrains.Annotations;
using UIKit;
using Xamarin.Forms;

namespace Autostop.Client.iOS.Adapters
{
	[UsedImplicitly]
	public class PageToViewControllerAdapter : IViewAdapter<UIViewController>
    {
        public UIViewController GetView<TViewModel>(IScreenFor<TViewModel> view)
        {
	        try
	        {
		        switch (view)
		        {
			        case Page page:
				        return page.CreateViewController();
			        case UIViewController resultView:
				        return resultView;
		        }
			}
	        catch (Exception e)
	        {
				Debug.WriteLine(e);
	        }

            return null;
        }

	    public UIViewController GetSearchView<TViewModel>(IScreenFor<TViewModel> view) where TViewModel : ISearchableViewModel
		{
			try
			{
				switch (view)
				{
					case Page page:
						var uiViewController = page.CreateViewController();
						uiViewController.CreateSearchViewOnNavigationBar(view.ViewModel);
						return uiViewController;
					case UIViewController resultView:
						return resultView;
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
			}

			return null;
	    }
    }
}