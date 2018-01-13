using System;
using Autostop.Client.Abstraction;
using Autostop.Client.Abstraction.Adapters;
using Autostop.Client.Abstraction.ViewModels;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Fragment = Android.App.Fragment;

namespace Autostop.Client.Android.Adapters
{
    public class PageToFragmentAdapter : IViewAdapter<Fragment>
    {
	    private readonly ICurrentActivity _currentActivity;

	    public PageToFragmentAdapter(ICurrentActivity currentActivity)
	    {
		    _currentActivity = currentActivity;
	    }
        public Fragment GetView<TViewModel>(IScreenFor<TViewModel> view)
        {
            try
            {
                switch (view)
                {
                    case ContentPage page:
                        return page.CreateFragment(_currentActivity.Activity);
                    case Fragment resultView:
                        return resultView;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return null;
        }

        public Fragment GetSearchView<TViewModel>(IScreenFor<TViewModel> view) where TViewModel : ISearchableViewModel
        {
            throw new NotImplementedException();
        }
    }
}