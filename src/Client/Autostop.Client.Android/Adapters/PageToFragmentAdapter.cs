using System;
using Autostop.Client.Abstraction;
using Autostop.Client.Abstraction.Adapters;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Android.Views;
using JetBrains.Annotations;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Fragment = Android.App.Fragment;

namespace Autostop.Client.Android.Adapters
{
    [UsedImplicitly]
    public class PageToFragmentAdapter : IViewAdapter<Fragment>
    {
        public Fragment GetView<TViewModel>(IScreenFor<TViewModel> view)
        {
            try
            {
                switch (view)
                {
                    case ContentPage page:
                        return page.CreateFragment(RootActivity.Instance);
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