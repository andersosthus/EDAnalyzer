namespace EDAnalyzer.Models.EDSC
{
	public class EdscConstants
	{
		public enum KnownStatus
		{
			All = 0,
			Known = 1,
			Unknown = 2
		}

		public enum OutputMode
		{
			Terse = 1,
			Verbose = 2
		}

		public enum ApiVersion
		{
			Ver1 = 1,
			Ver2 = 2
		}
	}
}