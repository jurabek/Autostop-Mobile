using System.Windows.Input;
using GalaSoft.MvvmLight.Helpers;
using UIKit;

namespace Autostop.Client.iOS.Extensions
{
    public static class CommandBindingExtensions
    {
        public static void BindCommand(this object view, UIButton button, ICommand command)
        {
            button.SetCommand("TouchUpInside", command);
        }
    }
}