using System;
using System.Collections.Generic;
using System.Text;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Abstraction.Publishers
{
    public interface ISelectedDestinationByMapPublisher : IPublisher<Address>
    {
    }
}
