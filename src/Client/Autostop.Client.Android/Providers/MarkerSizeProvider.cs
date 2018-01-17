using Autostop.Client.Abstraction.Providers;
using Plugin.CurrentActivity;

namespace Autostop.Client.Android.Providers
{
	public class MarkerSizeProvider : IMarkerSizeProvider
	{
		private readonly ICurrentActivity _currentActivity;

		public MarkerSizeProvider(ICurrentActivity currentActivity)
		{
			_currentActivity = currentActivity;
		}

		public int GetHeight(float zoomLevel) => (int)(30 * zoomLevel / 17 * _currentActivity.Activity.Resources.DisplayMetrics.Density);

		public int GetWidth(float zoomLevel) => (int)(15 * zoomLevel / 17 * _currentActivity.Activity.Resources.DisplayMetrics.Density);
	}
}