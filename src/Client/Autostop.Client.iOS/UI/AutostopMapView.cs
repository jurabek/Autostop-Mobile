using System.Collections.ObjectModel;
using Google.Maps;

namespace Autostop.Client.iOS.UI
{
    public class AutostopMapView : MapView
    {
        public ObservableCollection<Marker> Markers { get; set; }
    }
}