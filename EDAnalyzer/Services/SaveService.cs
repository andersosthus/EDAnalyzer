using System;
using System.IO;
using System.Threading;
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
				var retry = true;
				var tries = 0;

				do
				{
					try
					{
						using (var file = File.Create(Constants.DataFile))
						{
							Serializer.Serialize(file, data);
						}
						retry = false;
					}
					catch (IOException)
					{
						tries++;
						if (tries >= 3) retry = false;
						Thread.Sleep(TimeSpan.FromMilliseconds(200));
					}
				} while (retry);

			});
		}
	}
}