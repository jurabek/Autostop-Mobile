using System;
using System.Reactive.Linq;
using Autostop.Client.Abstraction;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.iOS.Constants;
using Autostop.Client.iOS.Extensions;
using GalaSoft.MvvmLight.Helpers;
using Google.Maps;
using JetBrains.Annotations;
using UIKit;
using Location = Autostop.Common.Shared.Models.Location;
using MapView = Autostop.Client.iOS.UI.MapView;

namespace Autostop.Client.iOS.Views.Passenger
{
	public abstract class BaseChooseOnMapViewController<TViewModel> : UIViewController, IScreenFor<TViewModel>
		where TViewModel : class, ISearchableViewModel, IMapViewModel
    {
		protected MapView MapView;
	    protected UIButton DoneButton;
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
            
		    this.SetBinding(
		            () => MapView.Camera,
		            () => ViewModel.CameraTarget, BindingMode.TwoWay)
		        .ConvertTargetToSource(location =>
		            CameraPosition.FromCamera(location.Latitude, location.Longitude, 16));

		    ViewModel.CameraPositionChanged = Observable
		        .FromEventPattern<EventHandler<GMSCameraEventArgs>, GMSCameraEventArgs>(
		            e => MapView.CameraPositionIdle += e,
		            e => MapView.CameraPositionIdle -= e)
		        .Select(e => e.EventArgs.Position.Target)
		        .Select(c => new Location(c.Latitude, c.Longitude));

		    ViewModel.CameraStartMoving = Observable
		        .FromEventPattern<EventHandler<GMSWillMoveEventArgs>, GMSWillMoveEventArgs>(
		            e => MapView.WillMove += e,
		            e => MapView.WillMove -= e)
		        .Select(e => e.EventArgs.Gesture);
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