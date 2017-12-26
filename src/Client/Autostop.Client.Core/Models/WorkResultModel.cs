using Autostop.Common.Shared.Models;

namespace Autostop.Client.Core.Models
{
    public class WorkResultModel : EmptyAutocompleteResultModel
	{
		public WorkResultModel(Address address)
		{
			if (address == null)
			{
				PrimaryText = "Add work address";
			}
			else
			{
				PrimaryText = "Work";
				SecondaryText = address.FormattedAddress;
				Address = address;
			}
			Icon = "work.png";
		}
    }
}
