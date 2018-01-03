//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Support.V4.App;
//using Android.Views;
//using Android.Widget;
//using Autostop.Client.Abstraction;
//using Autostop.Client.Abstraction.Adapters;
//using Autostop.Client.Abstraction.ViewModels.Passenger.Places;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.Android;
//using Fragment = Android.App.Fragment;

//namespace Autostop.Client.Android.Adapters
//{
//    public class PageToFragmentAdapter : IViewAdapter<Fragment>
//    {
//        public Fragment GetView<TViewModel>(IScreenFor<TViewModel> view)
//        {
//            try
//            {
//                switch (view)
//                {
//                    case ContentPage page:
//                        return page.CreateFragment(Forms.Context);
//                    case Fragment resultView:
//                        return resultView;
//                }
//            }
//            catch (Exception e)
//            {
//            }

//            return null;
//        }

//        public Fragment GetSearchView<TViewModel>(IScreenFor<TViewModel> view) where TViewModel : ISearchableViewModel
//        {
//            throw new NotImplementedException();
//        }
//    }
//}