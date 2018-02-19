using System;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Abstraction.ViewModels
{
    public interface IMapViewModel
    {
        IObservable<Location> MyLocationChanged { get; }

        IObservable<Location> CameraPositionChanged { get; set; }

        IObservable<bool> CameraStartMoving { get; set; }

        Location CameraTarget { get; set; }
    }
}
