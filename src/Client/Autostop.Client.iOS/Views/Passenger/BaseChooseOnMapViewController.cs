using System;
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
			View.AddSubviews(MapView, CenterPinImageView, DoneButton);

			SetupConstraints();

			var searchTextField = this.CreateSearchViewOnNavigationBar(ViewModel);
		    searchTextField.ClearButtonMode = UITextFieldViewMode.Never;
		    searchTextField.ShouldBeginEditing = _ => false;
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			EdgesForExtendedLayout = UIRectEdge.All;
			NavigationController.NavigationBar.Translucent = true;
			NavigationController.NavigationBar.BarTintColor = UIColor.Clear;
		}

		private void SetupConstraints()
		{
			NSLayoutConstraint.ActivateConstraints(new[]
			{
				MapView.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor),
				MapView.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor),
				MapView.WidthAnchor.ConstraintEqualTo(View.WidthAnchor),
				MapView.HeightAnchor.ConstraintEqualTo(View.HeightAnchor)
			});

			NSLayoutConstraint.ActivateConstraints(new[]
			{
				CenterPinImageView.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor),
				NSLayoutConstraint.Create(CenterPinImageView, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, View.SafeAreaLayoutGuide, NSLayoutAttribute.CenterY, (nfloat) 0.93, 0),
				CenterPinImageView.WidthAnchor.ConstraintEqualTo(40),
				CenterPinImageView.HeightAnchor.ConstraintEqualTo(40)
			});

			NSLayoutConstraint.ActivateConstraints(new[]
			{
				DoneButton.BottomAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.BottomAnchor, -10),
				DoneButton.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor, 10),
				DoneButton.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor, -10),
				DoneButton.HeightAnchor.ConstraintEqualTo(40)
			});
		}

		public TViewModel ViewModel { get; set; }

		protected abstract UIImage GetPinImage();

		protected abstract string GetDoneButtonTitle();
	}
}