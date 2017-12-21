using System;
using System.Collections.Generic;

namespace Google.Maps
{
    public class LatLngComparer : IEqualityComparer<LatLng>
    {
        private LatLngComparer(float epsilon)
        {
            Epsilon = epsilon;
        }

        public float Epsilon { get; }

        public bool Equals(LatLng x, LatLng y)
        {
            if (x == null || y == null) return false;

            if (Equals(x.Latitude, y.Latitude, Epsilon) == false)
                return false;

            if (Equals(x.Longitude, y.Longitude, Epsilon) == false)
                return false;

            return true;
        }

        public int GetHashCode(LatLng value)
        {
            return value.Latitude.GetHashCode() ^ value.Longitude.GetHashCode();
        }

        public static LatLngComparer Within(float epsilon)
        {
            return new LatLngComparer(epsilon);
        }

        private bool Equals(double a, double b, float epsilonParam)
        {
            var epsilon = Convert.ToDouble(epsilonParam);
            var absA = Math.Abs(a);
            var absB = Math.Abs(b);
            var diff = Math.Abs(a - b);

            if (a * b == 0)
                return diff < epsilon * epsilon;
            return diff / (absA + absB) < epsilon;
        }
    }
}