using System;
using System.Collections.Generic;
using System.Text;
using Autostop.Client.Core.ViewModels.Passenger;
using Autostop.Client.iOS.Constants;
using Autostop.Client.iOS.Extensions;
using UIKit;

namespace Autostop.Client.iOS.Views.Passenger
{
	public class ChooseDestinationOnMapViewController : BaseChooseOnMapViewController<ChooseDestinationOnMapViewModel>
	{
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			this.BindCommand(DoneButton, ViewModel.GoBack);
		}

		protected override UIImage GetPinImage()
		{
			return Icons.DestinationPin;
		}

		protected override string GetDoneButtonTitle()
		{
			return "SET DESTINATION";
		}
	}
}
