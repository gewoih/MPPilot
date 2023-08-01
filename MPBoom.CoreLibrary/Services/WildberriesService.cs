using MPBoom.Core.Enums;
using MPBoom.Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net;
using System.Text;

namespace MPBoom.Core.Services
{
	public class WildberriesService : IDisposable
	{
		private const string _getAllUrl = "https://advert-api.wb.ru/adv/v0/adverts?";
		private const string _getInfoUrl = "https://advert-api.wb.ru/adv/v0/advert?id=";
		private const string _getKeywordsUrl = "https://advert-api.wb.ru/adv/v1/stat/words?id=";
		private const string _getFullStatUrl = "https://advert-api.wb.ru/adv/v1/fullstat?id=";
		private const string _changeCPMUrl = "https://advert-api.wb.ru/adv/v0/cpm";
		private const string _startCampaignUrl = "https://advert-api.wb.ru/adv/v0/start";
		private const string _pauseCampaignUrl = "https://advert-api.wb.ru/adv/v0/pause";
		private const string _stopCampaignUrl = "https://advert-api.wb.ru/adv/v0/stop";
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

		public async Task<bool> ChangeAdvertStatus(int advertId, AdvertStatus newStatus)
		{
			string? query;
			if (newStatus == AdvertStatus.InProgress)
				query = $"{_startCampaignUrl}?id={advertId}";
			else if (newStatus == AdvertStatus.Stopped)
				query = $"{_pauseCampaignUrl}?id={advertId}";
			else if (newStatus == AdvertStatus.Finished)
				query = $"{_stopCampaignUrl}?id={advertId}";
			else
				throw new ArgumentException($"Передан некорректный статус для рекламной кампании: '{newStatus}'");

			var result = await _httpClient.GetAsync(query);

			if (result.StatusCode == HttpStatusCode.BadRequest)
				throw new ArgumentException($"Передан некорректный ID рекламной кампании: '{advertId}'");

			return result.IsSuccessStatusCode;
		}

		public async Task<IEnumerable<AdvertCampaign>> GetAdvertsAsync(AdvertStatus? status = null, AdvertType? type = null, int? count = null)
		{
			var advertCampaignsListQuery = GetAllAdvertsQuery(status, type, count);
			var campaigns = await GetAdvertsFromJson(advertCampaignsListQuery);
			await FillUpAdvertsInfo(campaigns);
			await FillUpAdvertsKeywords(campaigns);
			await FillUpAdvertsStatistics(campaigns);

            return campaigns;
		}
		
		public async Task<bool> ChangeCPM(AdvertCampaign advertCampaign, int newCPM)
		{
			if (newCPM < 50)
				throw new ArgumentException("Новое значение CPM должно быть > 50");

            var data = new
			{
				advertId = advertCampaign.AdvertId,
				type = (int)advertCampaign.Type,
				cpm = newCPM,
				param = advertCampaign.CategoryId
			};

            var jsonData = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            
			var response = await _httpClient.PostAsync(_changeCPMUrl, content);
			if (response.StatusCode == HttpStatusCode.BadRequest)
				throw new ArgumentException("Некорректно переданы параметры для изменения CPM");

			return response.IsSuccessStatusCode;
        }

		private static string GetAllAdvertsQuery(AdvertStatus? status, AdvertType? type, int? count)
		{
			var query = _getAllUrl;
			if (status is not null)
				query += $"status={status.Value}&";
			if (type is not null)
				query += $"type={type.Value}&";
			if (count is not null)
				query += $"limit={count.Value}&";

			return query;
		}

		private async Task<IEnumerable<AdvertCampaign>> GetAdvertsFromJson(string query)
		{
			var result = await _httpClient.GetAsync(query);
			var stringResult = await result.Content.ReadAsStringAsync();
			var jArray = JsonConvert.DeserializeObject<JArray>(stringResult);

			var advertCampaigns = new List<AdvertCampaign>();
			foreach (var element in jArray)
			{
				var newCampaign = new AdvertCampaign
				{
					CreatedDate = DateTimeOffset.Parse(element.Value<string>("createTime"), CultureInfo.InvariantCulture, DateTimeStyles.None),
					LastUpdateDate = DateTimeOffset.Parse(element.Value<string>("changeTime"), CultureInfo.InvariantCulture, DateTimeStyles.None),
					StartDate = DateTimeOffset.Parse(element.Value<string>("startTime"), CultureInfo.InvariantCulture, DateTimeStyles.None),
					EndDate = DateTimeOffset.Parse(element.Value<string>("endTime"), CultureInfo.InvariantCulture, DateTimeStyles.None),
					Name = element.Value<string>("name"),
					AdvertId = element.Value<int>("advertId"),
					Status = (AdvertStatus)element.Value<int>("status"),
					Type = (AdvertType)element.Value<int>("type"),
				};
				newCampaign.IsEnabled = newCampaign.Status == AdvertStatus.InProgress;
				newCampaign.IsAvailableToEnable = newCampaign.Status != AdvertStatus.Finished;

				advertCampaigns.Add(newCampaign);
			}

			return advertCampaigns;
		}

		private async Task FillUpAdvertsInfo(IEnumerable<AdvertCampaign> campaigns)
		{
			var jObjects = await GetJObjectsByHttpQueries(campaigns, _getInfoUrl);
			foreach (var jObject in jObjects)
			{
				if (jObject.ContainsKey("params"))
				{
					var findedCampaign = campaigns.First(campaign => campaign.AdvertId == jObject["advertId"].Value<int>());
					var jsonProducts = jObject["params"][0]["nms"];

					findedCampaign.CPM = jObject["params"][0]["price"].Value<int>();

					var subjectId = jObject["params"][0].Value<int?>("subjectId");
                    var setId = jObject["params"][0].Value<int?>("setId");
                    var menuId = jObject["params"][0].Value<int?>("menuId");

					if (subjectId is not null)
						findedCampaign.CategoryId = subjectId.Value;
					else if (setId is not null)
						findedCampaign.CategoryId = setId.Value;
					else if (menuId is not null)
						findedCampaign.CategoryId = menuId.Value;

                    if (jsonProducts.HasValues)
						findedCampaign.ProductArticle = jsonProducts[0]["nm"].Value<string>();
				}
			}
		}

		private async Task FillUpAdvertsKeywords(IEnumerable<AdvertCampaign> campaigns)
		{
			var jObjects = await GetJObjectsByHttpQueries(campaigns, _getKeywordsUrl);
			foreach (var jObject in jObjects)
			{
				if (jObject is not null && jObject["words"].Value<JObject>().ContainsKey("pluse"))
				{
					if (jObject["words"]["pluse"].HasValues)
					{
						var findedCampaign = campaigns.First(campaign => campaign.AdvertId == jObject["stat"][0]["advertId"].Value<int>());
						findedCampaign.Keyword = jObject["words"]["pluse"][0].Value<string>();
					}
				}
			}
		}

		private async Task FillUpAdvertsStatistics(IEnumerable<AdvertCampaign> campaigns)
		{
            var jObjects = await GetJObjectsByHttpQueries(campaigns, _getFullStatUrl);
            foreach (var jObject in jObjects)
            {
                if (jObject is not null)
                {
                    var findedCampaign = campaigns.First(campaign => campaign.AdvertId == jObject["advertId"].Value<int>());
					findedCampaign.TotalViews = jObject.Value<int>("views");
                    findedCampaign.Clicks = jObject.Value<int>("clicks");
                    findedCampaign.UniqueViews = jObject.Value<int>("unique_users");
                    findedCampaign.TotalSpent = jObject.Value<double>("sum");
                    findedCampaign.AddedToCart = jObject.Value<int>("atbs");
                    findedCampaign.Orders = jObject.Value<int>("orders");
                    findedCampaign.OrdersSum = jObject.Value<double>("sum_price");
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
				if (!string.IsNullOrEmpty(stringResult) && task.Result.IsSuccessStatusCode)
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
