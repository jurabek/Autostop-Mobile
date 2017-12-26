using System;
using System.Collections.Generic;
using System.Text;

namespace Autostop.Client.Core.Models
{
    public class SetLocationOnMapResultModel : EmptyAutocompleteResultModel
    {
	    public SetLocationOnMapResultModel()
	    {
		    Icon = "set_location.png";
		    PrimaryText = "Set location on map";
	    }
    }
}
