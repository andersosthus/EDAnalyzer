using System.Threading.Tasks;
using EDAnalyzer.Models;
using ReactiveUI;

namespace EDAnalyzer.Interfaces
{
	public interface ISaveService
	{
		Task SaveCommodities(ReactiveList<ItemLine> data);
	}
}