using MPPilot.Domain.Models.Adverts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MPPilot.Domain.Services.Autobidders
{
	public class AdvertsMarketService
	{
		private const string _url = "https://catalog-ads.wildberries.ru/api/v5/search?keyword=";
		private readonly HttpClient _httpClient;

		public AdvertsMarketService(IHttpClientFactory httpClientFactory)
		{
			_httpClient = httpClientFactory.CreateClient();
		}

		public async Task<AdvertMarketStatistics> GetAdvertMarketStatistics(string keyword, int advertId)
		{
			var marketAdverts = await GetMarketAdverts(keyword);
			var currentAdvert = marketAdverts.FirstOrDefault(a => a.AdvertId == advertId);

			int subject = 0;
			int advertPosition = 0;
			int advertCPM = 0;
			if (currentAdvert is not null)
			{
				advertPosition = marketAdverts.IndexOf(currentAdvert) + 1;
				advertCPM = currentAdvert.CPM;
				subject = currentAdvert.Subject;
			}

			return new AdvertMarketStatistics
			{
				MarketAdverts = marketAdverts,
				AdvertPosition = advertPosition,
				CPM = advertCPM,
				Subject = subject,
			};
		}

		private async Task<List<AdvertMarketInfo>> GetMarketAdverts(string keyword)
		{
			var result = await _httpClient.GetAsync(_url + keyword);
			var jsonResult = await result.Content.ReadAsStringAsync();
			var jObject = JsonConvert.DeserializeObject<JObject>(jsonResult);

			var advertsInfo = new List<AdvertMarketInfo>();
			var position = 1;
			foreach (var advert in jObject["adverts"])
			{
				var advertInfo = new AdvertMarketInfo
				{
					Position = position,
					AdvertId = advert["advertId"].Value<int>(),
					CPM = advert["cpm"].Value<int>(),
					Subject = advert["cpm"].Value<int>(),
				};

				position++;
				advertsInfo.Add(advertInfo);
			}

			return advertsInfo;
		}
	}
}
