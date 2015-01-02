using System.Collections.Generic;
using EDAnalyzer.Models;
using EDAnalyzer.Services;

namespace EDAnalyzer.Interfaces
{
	public interface ICalculateLoot
	{
		List<Trade> CalculateProfitInOneSystem(IList<ItemLine> itemsInSystem);
		void CalculateProfitAcrossSystems(IList<ItemLine> items);
	}
}