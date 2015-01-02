using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EDAnalyzer.Converter
{
	public class BoolToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null) return Visibility.Collapsed;

			bool valueAsBool;
			if (bool.TryParse(parameter as string, out valueAsBool))
			{
				return Convert(((bool) value) == valueAsBool);
			}

			try
			{
				valueAsBool = (bool) value;
			}
			catch (Exception)
			{
				return Visibility.Collapsed;
			}

			return Convert(valueAsBool);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		internal Visibility Convert(bool isVisible)
		{
			return isVisible ? Visibility.Visible : Visibility.Collapsed;
		}
	}
}