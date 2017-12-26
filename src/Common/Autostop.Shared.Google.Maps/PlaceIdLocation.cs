using System;
using System.Collections.Generic;
using System.Text;

namespace Google.Maps
{
    public class PlaceIdLocation : Location
    {
	    private readonly string _placeId;

	    public PlaceIdLocation(string placeId)
	    {
		    _placeId = placeId;
	    }

	    public override string GetAsUrlParameter()
	    {
		    return _placeId;
	    }
    }
}
