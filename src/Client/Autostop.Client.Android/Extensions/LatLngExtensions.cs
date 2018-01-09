using Android.Gms.Maps.Model;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Android.Extensions
{
    public static class LatLngExtensions
    {
        public static Location ToLocation(this LatLng latLng)
        {
            return new Location(latLng.Latitude, latLng.Longitude);
        }

        public static Location ToLocation(this Google.Maps.LatLng latLng)
        {
            return new Location(latLng.Latitude, latLng.Longitude);
        }
    }
}