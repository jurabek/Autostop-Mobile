using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Autostop.Client.Core.ViewModels.Passenger.ChooseOnMap;
using GalaSoft.MvvmLight.Helpers;

namespace Autostop.Client.Android.Fragments
{
	public class ChoseDestinationOnMapFragment : BaseChooseOnMapFragment<ChooseDestinationOnMapViewModel>
	{
		public override void OnViewCreated(View view, Bundle savedInstanceState)
		{
			base.OnViewCreated(view, savedInstanceState);
			DoneButton.SetCommand(nameof(Button.Click), ViewModel.Done);
		}
		protected override string GetDoneButtonTitle() => "Save destination address";
			
		protected override int GetPinImageResource() => Resource.Drawable.pin;
	}
}