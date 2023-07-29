using MPBoom.Services.PricesLoader.Models;
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

        public async Task<AdvertCampaignSearchPosition> GetAdvertCampaignsStatistics(string advertId, string keyword)
        {
            var result = await _httpClient.GetAsync(_url + keyword);
            var jsonResult = await result.Content.ReadAsStringAsync();
            var jObject = JsonConvert.DeserializeObject<JObject>(jsonResult);

            for (int i = 0; i < jObject["adverts"].Count(); i++)
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
    }
}
