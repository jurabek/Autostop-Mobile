using Autostop.Client.Core.ViewModels.Passenger;
using Autostop.Client.iOS.UI;
using Autostop.Client.iOS.Views.Passenger;
using CoreGraphics;
using UIKit;

namespace Autostop.Client.iOS.Extensions.MainView
{
    public static class MainViewExtensions
    {
        public static SearchPlaceTextField GetSearchText(this MainViewController controller,
            BaseSearchPlaceViewModel vm, UIViewController viewController)
        {
            viewController.EdgesForExtendedLayout = UIRectEdge.None;
            viewController.NavigationItem.HidesBackButton = true;
            var frame = new CGRect(0, 0, controller.NavigationController.NavigationBar.Frame.Size.Width - 20, 30);
            var searchTextField =
                new SearchPlaceTextField(frame, vm, () => controller.NavigationController.PopViewController(true));
            viewController.NavigationItem.TitleView = searchTextField;
            searchTextField.BecomeFirstResponder();
            return searchTextField;
        }
    }
}