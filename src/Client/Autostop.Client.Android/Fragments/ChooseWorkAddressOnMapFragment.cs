using Android.OS;
using Android.Views;
using Android.Widget;
using Autostop.Client.Core.ViewModels.Passenger.ChooseOnMap;
using GalaSoft.MvvmLight.Helpers;

namespace Autostop.Client.Android.Fragments
{
	public class ChooseWorkAddressOnMapFragment : BaseChooseOnMapFragment<ChooseWorkAddressOnMapViewModel>
	{
		public override void OnViewCreated(View view, Bundle savedInstanceState)
		{
			base.OnViewCreated(view, savedInstanceState);
			DoneButton.SetCommand(nameof(Button.Click), ViewModel.Done);
		}

		protected override string GetDoneButtonTitle() => "Save home address";

		protected override int GetPinImageResource() => Resource.Drawable.pin;
	}
}