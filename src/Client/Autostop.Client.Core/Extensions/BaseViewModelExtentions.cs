using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reflection;
using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.Core.ViewModels;

namespace Autostop.Client.Core.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class BaseViewModelExtentions
    {
        public static IObservable<TProperty> Changed<TProperty>(this BaseViewModel vm,
            string propertyName)
        {
            return vm.Changed
                .Where(c => c.PropertyName == propertyName)
                .Select(c => (TProperty) c.Sender
                    .GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .FirstOrDefault(p => p.Name == propertyName)
                    .GetValue(c.Sender));
        }

        public static IObservable<TProperty> Changed<TProperty>(this IObservableViewModel vm,
            Expression<Func<TProperty>> propertyExpression)
        {
            if (!(propertyExpression.Body is MemberExpression member))
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    propertyExpression.ToString()));

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    propertyExpression.ToString()));

            return vm.Changed.Where(c => c.PropertyName == propInfo.Name)
                .Select(c => propertyExpression.Compile().Invoke());
        }
    }
}