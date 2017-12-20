using System.Collections.ObjectModel;
using Google.Maps;

namespace Autostop.Client.iOS.UI
{
	class AutostopMapView : Google.Maps.MapView
	{
		private ObservableCollection<Marker> _markers;
		public ObservableCollection<Marker> Markers
		{
			get => _markers;
			set
			{
				_markers = value;
				// Create markers for current map
			}
		}
	}
}