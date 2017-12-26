using Autostop.Common.Shared.Models;

namespace Autostop.Client.Core.Models
{
    public class HomeResultModel : EmptyAutocompleteResultModel
    {
	    public HomeResultModel(Address address)
	    {
			if (address == null)
		    {
			    PrimaryText = "Add home address";
		    }
			else
			{
				PrimaryText = "Home";
				SecondaryText = address.FormattedAddress;
				Address = address;
			}
		    Icon = "home.png";
		}
    }
}
