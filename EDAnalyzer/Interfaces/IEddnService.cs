using System;
using EDAnalyzer.Models;

namespace EDAnalyzer.Interfaces
{
	public interface IEddnService
	{
		IObservable<ItemLine> FetchFromEddnAsync();
		void Disconnect();
	}
}