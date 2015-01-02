using System.IO;
using System.Threading.Tasks;
using EDAnalyzer.Interfaces;
using EDAnalyzer.Models;
using ProtoBuf;
using ReactiveUI;

namespace EDAnalyzer.Services
{
	public class SaveService : ISaveService
	{
		public async Task SaveCommodities(ReactiveList<ItemLine> data)
		{
			await Task.Factory.StartNew(() =>
			{
				using (var file = File.Create(Constants.DataFile))
				{
					Serializer.Serialize(file, data);
				}
			});
		}
	}
}