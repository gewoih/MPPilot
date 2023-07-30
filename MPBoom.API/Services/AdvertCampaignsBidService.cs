using MPBoom.CoreLibrary.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MPBoom.Services.PricesLoader.Services
{
    public class AdvertCampaignsBidService
    {
        private const string _url = "https://catalog-ads.wildberries.ru/api/v5/search?keyword=";
        private readonly HttpClient _httpClient;

        public AdvertCampaignsBidService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<int> GetAverageCPM(string keyword)
        {
            var jObject = await GetMarketAdverts(keyword);
            var advertsCount = jObject["adverts"].Count();

            var totalCPM = 0;
            foreach (var advert in jObject["adverts"])
            {
                totalCPM += advert["cpm"].Value<int>();
            }

            return totalCPM / advertsCount;
        }

        public async Task<AdvertCampaignSearchPosition> GetAdvertCampaignsStatistics(string advertId, string keyword)
        {
            var jObject = await GetMarketAdverts(keyword);
            var advertsCount = jObject["adverts"].Count();

            for (int i = 0; i < advertsCount; i++)
            {
                var advertJson = jObject["adverts"][i];
                if (advertJson["advertId"].Value<string>() == advertId)
                {
                    var searchPositionResult = new AdvertCampaignSearchPosition
                    {
                        AdPlace = i + 1
                    };

                    searchPositionResult.RealPlace = jObject["pages"][searchPositionResult.Page - 1]["positions"][i].Value<int>();

                    return searchPositionResult;
                }
            }

            return null;
        }

        private async Task<JObject> GetMarketAdverts(string keyword)
        {
            var result = await _httpClient.GetAsync(_url + keyword);
            var jsonResult = await result.Content.ReadAsStringAsync();
            var jObject = JsonConvert.DeserializeObject<JObject>(jsonResult);

            return jObject;
        }
    }
}
