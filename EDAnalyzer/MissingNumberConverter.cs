using System;
using CsvHelper.TypeConversion;

namespace EDAnalyzer
{
	public class MissingNumberConverter : DefaultTypeConverter
	{
		public override object ConvertFromString(TypeConverterOptions options, string text)
		{
			if (string.IsNullOrEmpty(text))
				return 0;

			Int32 result;
			if (int.TryParse(text, out result))
				return result;

			return base.ConvertFromString(options, text);
		}

		public override bool CanConvertFrom(Type type)
		{
			return type == typeof (string);
		}
	}
}