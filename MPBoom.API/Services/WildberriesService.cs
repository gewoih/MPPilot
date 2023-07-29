using MPBoom.Services.PricesLoader.Models;
using MPBoom.Services.PricesLoader.Models.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MPBoom.Services.PricesLoader.Services
{
    public class WildberriesService
    {
        private const string _getAdvertCampaignsUrl = "https://advert-api.wb.ru/adv/v0/adverts?";
        private const string _getAdvertCampaignInfoUrl = "https://advert-api.wb.ru/adv/v0/advert?id=";
        private readonly IHttpClientFactory _httpClientFactory;

        public WildberriesService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<AdvertCampaign>> GetAdvertCampaignsAsync(
            string apiKey,
            AdvertCampaignStatus? status = null,
            AdvertCampaignType? type = null)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", apiKey);

            var advertCampaignsListQuery = _getAdvertCampaignsUrl;
            if (status is not null)
                advertCampaignsListQuery += $"status={status.Value}&";
            if (type is not null)
                advertCampaignsListQuery += $"type={type.Value}&";

            var result = await httpClient.GetAsync(advertCampaignsListQuery);
            var stringResult = await result.Content.ReadAsStringAsync();

            var campaigns = await GetCampaignsFromJson(stringResult, apiKey);

            return campaigns;
        }

        private async Task<IEnumerable<AdvertCampaign>> GetCampaignsFromJson(string jsonAdvertsList, string apiKey)
        {
            var jArray = JsonConvert.DeserializeObject<JArray>(jsonAdvertsList);

            var advertCampaigns = new List<AdvertCampaign>();
            foreach (var element in jArray)
            {
                advertCampaigns.Add(new AdvertCampaign
                {
                    CreatedDate = DateTimeOffset.Parse(element.Value<string>("createTime")),
                    LastUpdateDate = DateTimeOffset.Parse(element.Value<string>("changeTime")),
                    StartDate = DateTimeOffset.Parse(element.Value<string>("startTime")),
                    EndDate = DateTimeOffset.Parse(element.Value<string>("endTime")),
                    Name = element.Value<string>("name"),
                    AdvertId = element.Value<string>("advertId"),
                    Status = (AdvertCampaignStatus)element.Value<int>("status"),
                    Type = (AdvertCampaignType)element.Value<int>("type")
                });
            }

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", apiKey);

            var httpTasks = new List<Task<HttpResponseMessage>>();
            foreach (var advertCampaign in advertCampaigns)
            {
                var query = _getAdvertCampaignInfoUrl + advertCampaign.AdvertId;
                httpTasks.Add(httpClient.GetAsync(query));
            }
            Task.WaitAll(httpTasks.ToArray());

            foreach (var task in httpTasks)
            {
                var stringResult = await task.Result.Content.ReadAsStringAsync();
                var jObject = JsonConvert.DeserializeObject<JObject>(stringResult);
                var findedCampaign = advertCampaigns.First(campaign => campaign.AdvertId == jObject["advertId"].Value<string>());
                findedCampaign.CPM = jObject["params"][0]["price"].Value<int>();
            }

            return advertCampaigns;
        }
    }
}
