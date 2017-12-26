using Autostop.Client.iOS.Renderers;
using Autostop.Client.Mobile.UI.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Page), typeof(AutostopXamarinFormsPageRenderer))]
namespace Autostop.Client.iOS.Renderers
{
	public class AutostopXamarinFormsPageRenderer : PageRenderer
	{
		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			NavigationController.NavigationBar.BarTintColor = PageHelper.GetBarTintColor(Element).ToUIColor();
			NavigationController.NavigationBar.Translucent = false;
		}
	}
}