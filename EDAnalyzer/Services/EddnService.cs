using System;
using System.Diagnostics;
using System.Net.Http;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EDAnalyzer.Interfaces;
using EDAnalyzer.Models;
using EDAnalyzer.Models.EDDN;
using Ionic.Zlib;
using NetMQ;
using NetMQ.Sockets;
using NetMQ.zmq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace EDAnalyzer.Services
{
	public class EddnService : IEddnService
	{
		private const string Endpoint = "tcp://eddn-relay.elite-markets.net:9500";
		private NetMQContext _ctx;
		private bool _run = true;
		private SubscriberSocket _sub;

		public EddnService()
		{
		}

		public IObservable<ItemLine> FetchFromEddnAsync(CancellationToken token)
		{
			return Observable.Create<ItemLine>(async observer =>
			{
				_ctx = NetMQContext.Create();
				_sub = _ctx.CreateSubscriberSocket();
				_sub.Subscribe("");
				_sub.Connect(Endpoint);

				do
				{
					var msg = new Msg();
					msg.InitEmpty();
					_sub.Receive(ref msg, SendReceiveOptions.None);

					if (msg.Data == null) continue;

					var uncompresed = ZlibStream.UncompressBuffer(msg.Data);
					var asString = Encoding.UTF8.GetString(uncompresed);

					var asJson = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<Root>(asString), token);

					var item = new ItemLine
					{
						SystemName = asJson.Message.SystemName.Trim(),
						StationName = asJson.Message.StationName.Trim(),
						CommodityName = asJson.Message.ItemName.Trim(),
						BuyPrice = asJson.Message.BuyPrice,
						SellPrice = asJson.Message.SellPrice,
						Demand = asJson.Message.Demand,
						UpdatedAt = asJson.Header.GatewayTimestamp
					};

					observer.OnNext(item);
				} while (!token.IsCancellationRequested);

				observer.OnCompleted();
			});
		}
	}
}