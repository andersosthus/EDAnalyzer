using System;
using System.Threading;
using EDAnalyzer.Models;

namespace EDAnalyzer.Interfaces
{
	public interface IEddnService
	{
		IObservable<ItemLine> FetchFromEddnAsync(CancellationToken token);
	}
}