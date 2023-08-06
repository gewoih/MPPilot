﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MPPilot.Domain.Services
{
    public class AdvertsBidService : IDisposable
    {
        private const string _url = "https://catalog-ads.wildberries.ru/api/v5/search?keyword=";
        private readonly HttpClient _httpClient;

        public AdvertsBidService(IHttpClientFactory httpClientFactory)
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

        private async Task<JObject> GetMarketAdverts(string keyword)
        {
            var result = await _httpClient.GetAsync(_url + keyword);
            var jsonResult = await result.Content.ReadAsStringAsync();
            var jObject = JsonConvert.DeserializeObject<JObject>(jsonResult);

            return jObject;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
