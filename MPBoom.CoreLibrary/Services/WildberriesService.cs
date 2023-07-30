using MPBoom.Core.Enums;
using MPBoom.Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MPBoom.Core.Services
{
	public class WildberriesService : IDisposable
	{
		private const string _getAdvertCampaignsUrl = "https://advert-api.wb.ru/adv/v0/adverts?";
		private const string _getAdvertCampaignInfoUrl = "https://advert-api.wb.ru/adv/v0/advert?id=";
		private const string _getAdvertCampaignKeywordsUrl = "https://advert-api.wb.ru/adv/v1/stat/words?id=";
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
			await FillUpAdvertCampaignsInfo(campaigns);
			await FillUpAdvertCampaignsKeywords(campaigns);

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
				var newCampaign = new AdvertCampaign
				{
					CreatedDate = DateTimeOffset.Parse(element.Value<string>("createTime")),
					LastUpdateDate = DateTimeOffset.Parse(element.Value<string>("changeTime")),
					StartDate = DateTimeOffset.Parse(element.Value<string>("startTime")),
					EndDate = DateTimeOffset.Parse(element.Value<string>("endTime")),
					Name = element.Value<string>("name"),
					AdvertId = element.Value<string>("advertId"),
					Status = (AdvertCampaignStatus)element.Value<int>("status"),
					Type = (AdvertCampaignType)element.Value<int>("type"),
				};
				newCampaign.IsEnabled = newCampaign.Status == AdvertCampaignStatus.InProgress;
				newCampaign.IsAvailableToEnable = newCampaign.Status != AdvertCampaignStatus.Finished;

				advertCampaigns.Add(newCampaign);
			}

			return advertCampaigns;
		}

		private async Task FillUpAdvertCampaignsInfo(IEnumerable<AdvertCampaign> campaigns)
		{
			var jObjects = await GetJObjectsByHttpQueries(campaigns, _getAdvertCampaignInfoUrl);
			foreach (var jObject in jObjects)
			{
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

		private async Task FillUpAdvertCampaignsKeywords(IEnumerable<AdvertCampaign> campaigns)
		{
			var jObjects = await GetJObjectsByHttpQueries(campaigns, _getAdvertCampaignKeywordsUrl);
			foreach (var jObject in jObjects)
			{
				if (jObject is not null && jObject["words"].Value<JObject>().ContainsKey("pluse"))
				{
					if (jObject["words"]["pluse"].HasValues)
					{
						var findedCampaign = campaigns.First(campaign => campaign.AdvertId == jObject["stat"][0]["advertId"].Value<string>());
						findedCampaign.Keyword = jObject["words"]["pluse"][0].Value<string>();
					}
				}
			}
		}

		private async Task<List<JObject>> GetJObjectsByHttpQueries(IEnumerable<AdvertCampaign> campaigns, string url)
		{
			var httpTasks = new List<Task<HttpResponseMessage>>();
			foreach (var advertCampaign in campaigns)
			{
				var query = url + advertCampaign.AdvertId;
				httpTasks.Add(_httpClient.GetAsync(query));
			}
			Task.WaitAll(httpTasks.ToArray());

			var jObjects = new List<JObject>();
			foreach (var task in httpTasks)
			{
				var stringResult = await task.Result.Content.ReadAsStringAsync();
				jObjects.Add(JsonConvert.DeserializeObject<JObject>(stringResult));
			}

			return jObjects;
		}

		public void Dispose()
		{
			_httpClient.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}
