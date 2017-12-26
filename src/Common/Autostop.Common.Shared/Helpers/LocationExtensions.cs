using System;
using System.Collections.Generic;
using System.Text;
using Autostop.Common.Shared.Models;
using static System.Math;

namespace Autostop.Common.Shared.Helpers
{
    public static class LocationExtensions
    {
	    public static double ToRadians(this double degrees)
	    {
		    return degrees * Math.PI / 180.0;
	    }

	    public static double ToDegrees(this double radians)
	    {
			return radians * 180.0 / Math.PI;
	    }


	    public static double BearingTo(this Location begin, Location end)
	    {
			double lat = Abs(begin.Latitude - end.Latitude);
		    double lng = Abs(begin.Longitude - end.Longitude);

		    if (begin.Latitude < end.Latitude && begin.Longitude < end.Longitude)
			    return Atan(lng / lat).ToDegrees();
		    else if (begin.Latitude >= end.Latitude && begin.Longitude < end.Longitude)
			    return 90 - Atan(lng / lat).ToDegrees() + 90;
		    else if (begin.Latitude >= end.Latitude && begin.Longitude >= end.Longitude)
			    return Atan(lng / lat).ToDegrees() + 180;
		    else if (begin.Latitude < end.Latitude && begin.Longitude >= end.Longitude)
			    return 90 - Atan(lng / lat).ToDegrees() + 270;
		    return -1;
		}
	}
}
