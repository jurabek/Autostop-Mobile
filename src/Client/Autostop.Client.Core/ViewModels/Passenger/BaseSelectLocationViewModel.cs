using System;
using System.Collections.Generic;
using System.Text;

namespace Autostop.Client.Core.ViewModels.Passenger
{
    public abstract class BaseSelectLocationViewModel : BaseViewModel
    {
        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set => RaiseAndSetIfChanged(ref _searchText, value);
        }
    }
}
