using System;
using Autofac;
using Autostop.Client.Abstraction;
using Autostop.Client.Abstraction.Factories;

namespace Autostop.Client.Core.Factories
{
    public class ViewFactory : IViewFactory
    {
        private readonly IContainer _container = BootstrapperBase.Container;

        public virtual IScreenFor<TViewModel> CreateView<TViewModel>(TViewModel vm)
        {
            var viewType = typeof(IScreenFor<>).MakeGenericType(vm.GetType());
            if (!(_container.Resolve(viewType) is IScreenFor<TViewModel> view))
                throw new ArgumentException($"Resolve type {typeof(TViewModel).Name} does not implement IScreen");

            view.ViewModel = vm;

            return view;
        }
    }
}