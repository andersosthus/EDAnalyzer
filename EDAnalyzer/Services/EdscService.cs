using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EDAnalyzer.Models.EDSC;
using EDAnalyzer.Models.EDSC.Query;
using EDAnalyzer.Models.EDSC.Response;
using Newtonsoft.Json;

namespace EDAnalyzer.Services
{
	public class EdscService : IQueryEdsc
	{
		private const string BaseUrl = "http://edstarcoordinator.com/api.asmx/";
		private readonly HttpClient _client;

		public EdscService()
		{
			_client = new HttpClient();
		}

		public async Task<EdscQueryResponse> QueryForSystemNameAsync(string systemName)
		{
			var queryContent = CreateQueryForSystemNamePayload(systemName);
			var result = await _client.PostAsync(BaseUrl + "GetSystems", queryContent);
			var responseContent = await result.Content.ReadAsStringAsync();
			var response = JsonConvert.DeserializeObject<EdscQueryResponse>(responseContent);

			return response;
		}

		public async Task<EdscSystemsInRangeResponse> QueryForSystemsWithinRangeAsync(float lightYearsRadius, float[] origin)
		{
			var queryContent = CreateQueryForSystemsWithinRange(null, lightYearsRadius, origin);
			var result = await _client.PostAsync(BaseUrl + "GetDistances", queryContent);
			var responseContent = await result.Content.ReadAsStringAsync();
			var response = JsonConvert.DeserializeObject<EdscSystemsInRangeResponse>(responseContent);

			return response;
		}

		private static StringContent CreateQueryForSystemsWithinRange(string systemName, float lightYearsRadius, float[] origin)
		{
			var query = CreateQueryObject(systemName);
			query.Query.QueryFilter.CoordSphere = new CoordSphere
			{
				Radius = lightYearsRadius,
				Origin = origin
			};

			var queryJson = JsonConvert.SerializeObject(query);
			var queryContent = new StringContent(queryJson, Encoding.UTF8, "application/json");
			return queryContent;
		}

		private static StringContent CreateQueryForSystemNamePayload(string systemName)
		{
			var query = CreateQueryObject(systemName);

			var queryJson = JsonConvert.SerializeObject(query);
			var queryContent = new StringContent(queryJson, Encoding.UTF8, "application/json");
			return queryContent;
		}

		private static QueryForSystems CreateQueryObject(string systemName)
		{
			var query = new QueryForSystems
			{
				Query = new QueryParams
				{
					ApiVersion = EdscConstants.ApiVersion.Ver2,
					OutputMode = EdscConstants.OutputMode.Verbose,
					UseTestDb = true,
					QueryFilter = new QueryFilter
					{
						ConfidenceRating = 1,
						KnownStatus = EdscConstants.KnownStatus.All,
						SystemNameToQuery = systemName,
						CoordSphere = null
					}
				}
			};
			return query;
		}
	}

	public interface IQueryEdsc
	{
		Task<EdscQueryResponse> QueryForSystemNameAsync(string systemName);
		Task<EdscSystemsInRangeResponse> QueryForSystemsWithinRangeAsync(float lightYearsRadius, float[] origin);
	}
}