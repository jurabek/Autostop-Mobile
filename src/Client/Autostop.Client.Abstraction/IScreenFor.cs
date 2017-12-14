using System;
using System.Collections.Generic;
using System.Text;

namespace Autostop.Client.Abstraction
{
    public interface IScreenFor<TViewModel>
    {
        TViewModel ViewModel { get; set; }
    }
}
