using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace Autostop.Client.Core.ViewModels
{
	public abstract class ViewModelBase : INotifyPropertyChanged
	{
		protected ViewModelBase()
		{
			Changed = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>
				(e => PropertyChanged += e,
				e => PropertyChanged -= e)
				.Select(e => new ObservablePropertyChangedEventArgs<ViewModelBase>(e.EventArgs.PropertyName, this));
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected virtual void RaiseAndSetIfChanged<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = null)
		{
			if(EqualityComparer<T>.Default.Equals(oldValue, newValue))
				return;

			oldValue = newValue;
			RaisePropertyChanged(propertyName ?? string.Empty);
		}

		public IObservable<ObservablePropertyChangedEventArgs<ViewModelBase>> Changed { get; }
	}

	public class ObservablePropertyChangedEventArgs<TSender>
	{
		public ObservablePropertyChangedEventArgs(string propertyName, TSender sender)
		{
			PropertyName = propertyName;
			Sender = sender;
		}

		public string PropertyName { get; }

		public TSender Sender { get; }
	}
}
