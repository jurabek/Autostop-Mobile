using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Autostop.Client.Core.ViewModels
{
    public abstract class BaseViewModel : ObservableObject, IDisposable
    {
        protected BaseViewModel()
        {
            Changed = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>
                (e => PropertyChanged += e,
                    e => PropertyChanged -= e)
                .Select(e => new ObservablePropertyChangedEventArgs<BaseViewModel>(this, e.EventArgs.PropertyName));
        }

        public IObservable<ObservablePropertyChangedEventArgs<BaseViewModel>> Changed { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void RaiseAndSetIfChanged<T>(ref T oldValue, T newValue,
            [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
                return;

            oldValue = newValue;
            RaisePropertyChanged(propertyName ?? string.Empty);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        public virtual Task Load()
        {
            return Task.CompletedTask;
        }
    }
}