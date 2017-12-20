using System;
using System.Collections.ObjectModel;
using Google.Maps;

namespace Autostop.Client.iOS.Views.Passenger
{
    public partial class AutostopMapView : MapView
    {
        public AutostopMapView()
        {   
        }
        public AutostopMapView(IntPtr handle) : base(handle)
        {
        }

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