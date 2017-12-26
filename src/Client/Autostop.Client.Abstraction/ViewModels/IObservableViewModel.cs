using System;
using System.ComponentModel;

namespace Autostop.Client.Abstraction.ViewModels
{
	public interface IObservableViewModel : INotifyPropertyChanged
	{
		IObservable<ObservablePropertyChangedEventArgs<IObservableViewModel>> Changed { get; }
	}
}