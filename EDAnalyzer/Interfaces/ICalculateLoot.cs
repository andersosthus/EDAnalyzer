using System.Collections.Generic;
using System.Threading.Tasks;
using EDAnalyzer.Models;

namespace EDAnalyzer.Interfaces
{
	public interface ICalculateLoot
	{
		Task<List<Trade>> CalculateProfitInOneSystem(IList<ItemLine> itemsInSystem);
		Task<List<Trade>> CalculateProfitAcrossSystems(IList<ItemLine> items);
		Task<List<Trade>> CalculateProfitAcrossSeveralSystemsAsync(IList<ItemLine> items, IList<string> systemNames);
	}
}