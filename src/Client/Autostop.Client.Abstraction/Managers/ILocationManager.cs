using System;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Abstraction.Managers
{
    public interface ILocationManager : IDisposable
    {
        IObservable<Location> LocationChanged { get; }

        IObservable<double> HeadingChanged { get; }

        Location Location { get; }

        double Course { get; }

        void StartUpdatingLocation();

        void StopUpdatingLocation();
    }
}