using System;

namespace EDAnalyzer
{
	public static class Constants
	{
		public static string Filename = "data.bin";
		public static string DataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
		                                "\\EDAnalyzer";
		public static string DataFile = DataPath + "\\" + Filename;

		public enum TradeType
		{
			IntraSystem = 0,
			CrossSystem = 1
		}
	}
}