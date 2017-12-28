using Autostop.Client.Core.ViewModels.Passenger;
using Autostop.Client.iOS.Constants;
using JetBrains.Annotations;
using UIKit;

namespace Autostop.Client.iOS.Views.Passenger
{
    [UsedImplicitly]
    public class ChooseWorkAddressOnMapViewController : BaseChooseOnMapViewController<ChooseWorkAddressOnMapViewModel>
    {
        protected override UIImage GetPinImage() => Icons.DefaultPin;

        protected override string GetDoneButtonTitle() => "Save";

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();
            await ViewModel.Load();
        }
    }
}
