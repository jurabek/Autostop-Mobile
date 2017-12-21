using System.Windows.Input;
using Autostop.Client.iOS.Views.Passenger;
using GalaSoft.MvvmLight.Helpers;
using UIKit;

namespace Autostop.Client.iOS.Extensions
{
    public static class CommandBindingExtensions
    {
        public static void BindCommand(this MainViewController view, UIButton button, ICommand command)
        {
            button.SetCommand("TouchUpInside", command);
        }
    }
}