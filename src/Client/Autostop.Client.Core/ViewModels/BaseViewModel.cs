using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace Autostop.Client.Core.ViewModels
{
	public abstract class BaseViewModel : GalaSoft.MvvmLight.ViewModelBase, IDisposable
	{
		protected BaseViewModel()
		{
			Changed = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>
				(e => PropertyChanged += e,
				e => PropertyChanged -= e)
				.Select(e => new ObservablePropertyChangedEventArgs<BaseViewModel>(this, e.EventArgs.PropertyName));
		}

		protected virtual void RaiseAndSetIfChanged<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = null)
		{
			if(EqualityComparer<T>.Default.Equals(oldValue, newValue))
				return;

			oldValue = newValue;
			RaisePropertyChanged(propertyName ?? string.Empty);
		}

		public IObservable<ObservablePropertyChangedEventArgs<BaseViewModel>> Changed { get; }

	    public void Dispose()
	    {
            Dispose(true);
	        GC.SuppressFinalize(this);
        }

	    public virtual void Dispose(bool disposing)
	    {
	    }
	}

	public class ObservablePropertyChangedEventArgs<TSender>
	{
		public ObservablePropertyChangedEventArgs(TSender sender, string propertyName)
		{
			PropertyName = propertyName;
			Sender = sender;
		}

		public string PropertyName { get; }

		public TSender Sender { get; }
	}
}
