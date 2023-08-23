using MPPilot.Domain.Models.Adverts;
using MPPilot.Domain.Services.Accounts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace MPPilot.Domain.Services.Marketplaces
{
	public class WildberriesService
	{
		private const string _baseUrl = "https://advert-api.wb.ru/adv/";
		private readonly HttpClient _httpClient;

		public WildberriesService(IHttpClientFactory httpClientFactory, IAccountsService accountsService)
		{
			_httpClient = httpClientFactory.CreateClient();
			_httpClient.BaseAddress = new Uri(_baseUrl);

			var currentAccountSettings = accountsService.GetSettingsAsync().Result;
			if (currentAccountSettings is not null && currentAccountSettings.WildberriesApiKey is not null)
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(currentAccountSettings.WildberriesApiKey);
		}

		public void SetApiKey(string apiKey)
		{
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(apiKey);
		}

		public async Task<double> GetExpensesForToday(int advertId)
		{
			var query = $"v1/fullstat?id={advertId}";
			var result = await _httpClient.GetAsync(query);
			var jsonResult = await result.Content.ReadAsStringAsync();

			if (string.IsNullOrEmpty(jsonResult))
				return 0;

			var jObject = JObject.Parse(jsonResult);

			if (jObject["days"] is null)
				return 0;

			var lastDay = jObject["days"].Last;
			var date = lastDay.Value<DateTime>("date");

			if (date.Date == DateTimeOffset.Now.Date)
				return lastDay.Value<double>("sum");
			else
				return 0;
		}

		public async Task<bool> ChangeAdvertKeyword(int advertId, string newKeyword)
		{
			var data = new { pluse = Array.Empty<string>() };

			if (!string.IsNullOrEmpty(newKeyword))
				data = new { pluse = new string[] { newKeyword } };

			var serializedData = JsonConvert.SerializeObject(data);
			var content = new StringContent(serializedData, Encoding.UTF8, "application/json");

			var query = $"v1/search/set-plus?id={advertId}";
			var result = await _httpClient.PostAsync(query, content);
			if (result.StatusCode == HttpStatusCode.BadRequest)
				throw new ArgumentException($"Произошла ошибка при изменении ключевой фразы. {nameof(newKeyword)} = '{newKeyword}'");

			return result.IsSuccessStatusCode;
		}

		public async Task<bool> RenameAdvert(int advertId, string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			var serializedData = JsonConvert.SerializeObject(new { advertId, name });
			var content = new StringContent(serializedData, Encoding.UTF8, "application/json");

			var result = await _httpClient.PostAsync("v0/rename", content);

			if (result.StatusCode == HttpStatusCode.BadRequest)
				throw new ArgumentException($"Неверный формат запроса для изменения названия РК. {nameof(advertId)}={advertId}; {nameof(name)}={name}");

			return result.IsSuccessStatusCode;
		}

		public async Task<bool> StartAdvertAsync(int advertId)
		{
			var query = $"v0/start?id={advertId}";
			var result = await _httpClient.GetAsync(query);

			if (result.StatusCode == HttpStatusCode.BadRequest)
				throw new ArgumentException($"Передан некорректный ID рекламной кампании: '{advertId}'");

			return result.IsSuccessStatusCode;
		}

		public async Task<bool> StopAdvertAsync(int advertId)
		{
			var query = $"v0/pause?id={advertId}";
			var result = await _httpClient.GetAsync(query);

			if (result.StatusCode == HttpStatusCode.BadRequest)
				throw new ArgumentException($"Передан некорректный ID рекламной кампании: '{advertId}'");

			return result.IsSuccessStatusCode;
		}

		public async Task<Advert> GetAdvertWithKeywordAndCPM(int advertId)
		{
			var adverts = new List<Advert> { new Advert { AdvertId = advertId } };
			await LoadInfo(adverts);
			await LoadKeywords(adverts);

			return adverts.First();
		}

		public async Task<List<Advert>> GetActiveAdvertsAsync(bool withInfo = false, bool withKeywords = false, bool withStatistics = false, int? count = null)
		{
			var productPageAdvertsQuery = GetAllAdvertsQuery(AdvertType.ProductPage, count);
			var searchAdvertsQuery = GetAllAdvertsQuery(AdvertType.Search, count);

			var productPageAdvertsTask = GetAdverts(productPageAdvertsQuery);
			var searchAdvertsTask = GetAdverts(searchAdvertsQuery);

			await Task.WhenAll(productPageAdvertsTask, searchAdvertsTask);

			var adverts = productPageAdvertsTask.Result.Concat(searchAdvertsTask.Result).ToList();

			if (withInfo)
				await LoadInfo(adverts);
			if (withKeywords)
				await LoadKeywords(adverts);
			if (withStatistics)
				await LoadStatistics(adverts);

			return adverts;
		}

		public async Task<bool> ChangeCPM(Advert advert, int newCPM)
		{
			if (newCPM < 50)
				throw new ArgumentException("Новое значение CPM должно быть > 50");

			var jsonData = JsonConvert.SerializeObject(new
			{
				advertId = advert.AdvertId,
				type = (int)advert.Type,
				cpm = newCPM,
				param = advert.CategoryId
			});

			var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync("v0/cpm", content);

			if (response.StatusCode == HttpStatusCode.BadRequest)
				throw new ArgumentException("Некорректно переданы параметры для изменения CPM");

			return response.IsSuccessStatusCode;
		}

		private static string GetAllAdvertsQuery(AdvertType? type, int? count)
		{
			var query = "v0/adverts?";
			if (type is not null)
				query += $"type={(int)type}&";
			if (count is not null)
				query += $"limit={count}&";

			return query;
		}

		private async Task<List<Advert>> GetAdverts(string query)
		{
			var result = await _httpClient.GetAsync(query);

			result.EnsureSuccessStatusCode();

			var stringResult = await result.Content.ReadAsStringAsync();
			var jArray = JsonConvert.DeserializeObject<JArray>(stringResult);

			var advertCampaigns = new List<Advert>();
			foreach (var element in jArray)
			{
				var newCampaign = new Advert
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

				if (newCampaign.Status != AdvertStatus.Finished)
					advertCampaigns.Add(newCampaign);
			}

			return advertCampaigns;
		}

		private async Task LoadInfo(List<Advert> adverts)
		{
			var jObjects = await GetJsonByAdverts(adverts, "v0/advert?id=");
			foreach (var jObject in jObjects)
			{
				if (jObject.ContainsKey("params"))
				{
					var foundAdvert = adverts.First(advert => advert.AdvertId == jObject["advertId"].Value<int>());
					var jsonProducts = jObject["params"][0]["nms"];

					foundAdvert.CPM = jObject["params"][0]["price"].Value<int>();

					var subjectId = jObject["params"][0].Value<int?>("subjectId");
					var setId = jObject["params"][0].Value<int?>("setId");
					var menuId = jObject["params"][0].Value<int?>("menuId");

					if (subjectId is not null)
						foundAdvert.CategoryId = subjectId.Value;
					else if (setId is not null)
						foundAdvert.CategoryId = setId.Value;
					else if (menuId is not null)
						foundAdvert.CategoryId = menuId.Value;

					foundAdvert.Type = (AdvertType)jObject["type"].Value<int>();

					if (jsonProducts.HasValues)
						foundAdvert.ProductArticle = jsonProducts[0]["nm"].Value<string>();
				}
			}
		}

		private async Task<List<Advert>> LoadKeywords(List<Advert> adverts)
		{
			var jObjects = await GetJsonByAdverts(adverts, "v1/stat/words?id=");
			foreach (var jObject in jObjects)
			{
				if (jObject is not null && jObject["words"].Value<JObject>().ContainsKey("pluse"))
				{
					if (jObject["words"]["pluse"].HasValues)
					{
						var advertId = jObject.Value<int>("advertId");
						var foundAdvert = adverts.First(advert => advert.AdvertId == advertId);
						foundAdvert.Keyword = jObject["words"]["pluse"][0].Value<string>();
					}
				}
			}

			return adverts;
		}

		private async Task LoadStatistics(IEnumerable<Advert> adverts)
		{
			var jObjects = await GetJsonByAdverts(adverts, "v1/fullstat?id=");
			foreach (var jObject in jObjects)
			{
				if (jObject is not null)
				{
					var foundAdvert = adverts.First(campaign => campaign.AdvertId == jObject["advertId"].Value<int>());
					foundAdvert.TotalViews = jObject.Value<int>("views");
					foundAdvert.Clicks = jObject.Value<int>("clicks");
					foundAdvert.UniqueViews = jObject.Value<int>("unique_users");
					foundAdvert.TotalSpent = jObject.Value<double>("sum");
					foundAdvert.AddedToCart = jObject.Value<int>("atbs");
					foundAdvert.Orders = jObject.Value<int>("orders");
					foundAdvert.OrdersSum = jObject.Value<double>("sum_price");
				}
			}
		}

		private async Task<List<JObject>> GetJsonByAdverts(IEnumerable<Advert> adverts, string url)
		{
			var jObjects = new List<JObject>();
			var semaphore = new SemaphoreSlim(50);

			var httpTasks = adverts.Select(async advert =>
			{
				await semaphore.WaitAsync();
				try
				{
					var query = url + advert.AdvertId;
					var response = await _httpClient.GetAsync(query);

					if (response.IsSuccessStatusCode)
					{
						var stringResult = await response.Content.ReadAsStringAsync();
						var jObject = JsonConvert.DeserializeObject<JObject>(stringResult);

						if (jObject is not null)
							jObject["advertId"] = advert.AdvertId;

						jObjects.Add(jObject);
					}
					else
						throw new Exception(response.StatusCode.ToString());
				}
				finally
				{
					semaphore.Release();
				}
			});

			await Task.WhenAll(httpTasks);

			return jObjects;
		}
	}
}
