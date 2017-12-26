using System;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Abstraction.Managers
{
    public interface ILocationManager : IDisposable
    {
        IObservable<Location> LocationChanged { get; }

        Location Location { get; }

        void StartUpdatingLocation();

        void StopUpdatingLocation();
    }
}