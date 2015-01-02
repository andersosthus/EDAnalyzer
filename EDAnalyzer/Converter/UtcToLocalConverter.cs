using System;
using System.Globalization;
using System.Windows.Data;

namespace EDAnalyzer.Converter
{
	public class UtcToLocalConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null) return DateTimeOffset.Now;

			var timestamp = (DateTime)value;
			return timestamp.ToLocalTime();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}