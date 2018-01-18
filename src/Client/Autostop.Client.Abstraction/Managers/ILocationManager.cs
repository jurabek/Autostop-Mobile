using System;
using System.Threading.Tasks;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Abstraction.Managers
{
    public interface ILocationManager : IDisposable
    {
        IObservable<Location> LocationChanged { get; }

        Location LastKnownLocation { get; }

        void RequestLocationUpdates();

        void StopLocationUpdates();

        Task<Location> RequestSingleLocationUpdate();
    }
}