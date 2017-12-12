using System;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Abstraction.Managers
{
    public interface ILocationManager
    {
	    void StartUpdatingLocation();

	    void StopUpdatingLocation();

	    IObservable<Location> LocationChanged { get; }
    }
}
