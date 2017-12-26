using Autostop.Client.Abstraction;
using Autostop.Client.Abstraction.ViewModels.Passenger.Places;
using Autostop.Client.iOS.Constants;
using Autostop.Client.iOS.Extensions;
using Autostop.Client.iOS.UI;
using JetBrains.Annotations;
using UIKit;

namespace Autostop.Client.iOS.Views.Passenger
{
	public abstract class BaseChooseOnMapViewController<TViewModel> : UIViewController, IScreenFor<TViewModel>
		where TViewModel : class, ISearchableViewModel
	{
		[UsedImplicitly] protected MapView MapView;
		[UsedImplicitly] protected UIButton DoneButton;
		[UsedImplicitly] protected UIImageView CenterPinImageView;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			MapView = new MapView();
			
			DoneButton = new UIButton
			{
				BackgroundColor = Colors.PickupButtonColor,
				TranslatesAutoresizingMaskIntoConstraints = false
			};
			DoneButton.SetTitle(GetDoneButtonTitle(), UIControlState.Normal);

			CenterPinImageView = new UIImageView(GetPinImage()) { TranslatesAutoresizingMaskIntoConstraints = false };

			this.CreateSearchViewOnNavigationBar(ViewModel);
		}

		public TViewModel ViewModel { get; set; }

		protected abstract UIImage GetPinImage();

		protected abstract string GetDoneButtonTitle();
	}
}