using System;
using System.Globalization;
using Xamarin.Forms;

namespace Autostop.Client.Shared.UI.Converters
{
	public class EmptyToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return !string.IsNullOrEmpty(value as string);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
