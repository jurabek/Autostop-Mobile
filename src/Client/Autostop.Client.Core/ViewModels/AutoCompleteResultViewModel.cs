using System;
using System.Collections.Generic;
using System.Text;
using Autostop.Client.Abstraction.ViewModels;

namespace Autostop.Client.Core.ViewModels
{
    public class AutoCompleteResultViewModel : BaseViewModel, IAutoCompleteResultViewModel
	{
		private string _primaryText;

		public string PrimaryText
		{
			get => _primaryText;
			set => _primaryText = value;
		}

		private string _secondaryText;

		public string SecondaryText
		{
			get => _secondaryText;
			set => _secondaryText = value;
		}

		private string _placeId;

		public string PlaceId
		{
			get => _placeId;
			set => _placeId = value;
		}

		public string Icon { get; set; }

	}
}
