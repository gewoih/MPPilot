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
        private readonly HttpClient _httpClient;

        public WildberriesService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public void SetApiKey(string apiKey)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", apiKey);
        }

        public async Task<IEnumerable<AdvertCampaign>> GetAdvertCampaignsAsync(AdvertCampaignStatus? status = null, AdvertCampaignType? type = null)
        {
            var advertCampaignsListQuery = GetQueryForAdvertCampaignsList(status, type);
            var campaigns = await GetCampaignsFromJson(advertCampaignsListQuery);
            await FillUpAdvertCampaignsCPM(campaigns);

            return campaigns;
        }

        private static string GetQueryForAdvertCampaignsList(AdvertCampaignStatus? status, AdvertCampaignType? type)
        {
            var query = _getAdvertCampaignsUrl;
            if (status is not null)
                query += $"status={status.Value}&";
            if (type is not null)
                query += $"type={type.Value}&";

            return query;
        }

        private async Task<IEnumerable<AdvertCampaign>> GetCampaignsFromJson(string query)
        {
            var result = await _httpClient.GetAsync(query);
            var stringResult = await result.Content.ReadAsStringAsync();
            var jArray = JsonConvert.DeserializeObject<JArray>(stringResult);

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

            return advertCampaigns;
        }
        
        private async Task FillUpAdvertCampaignsCPM(IEnumerable<AdvertCampaign> campaigns)
        {
            var httpTasks = new List<Task<HttpResponseMessage>>();
            foreach (var advertCampaign in campaigns)
            {
                var query = _getAdvertCampaignInfoUrl + advertCampaign.AdvertId;
                httpTasks.Add(_httpClient.GetAsync(query));
            }
            Task.WaitAll(httpTasks.ToArray());

            foreach (var task in httpTasks)
            {
                var stringResult = await task.Result.Content.ReadAsStringAsync();
                var jObject = JsonConvert.DeserializeObject<JObject>(stringResult);

                if (jObject.ContainsKey("params"))
                {
                    var findedCampaign = campaigns.First(campaign => campaign.AdvertId == jObject["advertId"].Value<string>());
                    var jsonProducts = jObject["params"][0]["nms"];

                    findedCampaign.CPM = jObject["params"][0]["price"].Value<int>();
                    findedCampaign.Products = new List<Product>();
                    foreach (var jsonProduct in jsonProducts)
                    {
                        findedCampaign.Products.Add(new Product
                        {
                            Article = jsonProduct["nm"].Value<string>()
                        });
                    }
                }
            }
        }
    }
}
