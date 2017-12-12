using System;
using System.Collections.Generic;
using System.Text;

namespace Autostop.Common.Shared.Models
{
    public class Location
    {
	    public Location(double latitude, double longitude)
	    {
			Coordinate = new Coordinate(latitude, longitude);    
	    }

	    public Location(Coordinate coordinate)
	    {
		    Coordinate = coordinate;
	    }
		
	    public virtual Coordinate Coordinate { get; }
	}
}
