﻿using MPPilot.Domain.Exceptions;
using MPPilot.Domain.Models.Adverts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net;
using System.Text;

namespace MPPilot.Domain.Services
{
    public class WildberriesService : IDisposable
    {
        private const string _getAllUrl = "https://advert-api.wb.ru/adv/v0/adverts?";
        private const string _getInfoUrl = "https://advert-api.wb.ru/adv/v0/advert?id=";
        private const string _getKeywordsUrl = "https://advert-api.wb.ru/adv/v1/stat/words?id=";
        private const string _getFullStatUrl = "https://advert-api.wb.ru/adv/v1/fullstat?id=";
        private const string _changeAdvertCPMUrl = "https://advert-api.wb.ru/adv/v0/cpm";
        private const string _startAdvertUrl = "https://advert-api.wb.ru/adv/v0/start";
        private const string _pauseAdvertUrl = "https://advert-api.wb.ru/adv/v0/pause";
        private const string _renameAdvertUrl = "https://advert-api.wb.ru/adv/v0/rename";
        private const string _changeAdvertKeywordUrl = "https://advert-api.wb.ru/adv/v1/search/set-plus";
        private const string _invalidApiKeyMessage = "Задан некорректный API-ключ";
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

        public async Task<bool> ChangeAdvertKeyword(int advertId, string newKeyword)
        {
            var data = new { pluse = Array.Empty<string>() };

            if (!string.IsNullOrEmpty(newKeyword))
                data = new { pluse = new string[] { newKeyword } };

            var serializedData = JsonConvert.SerializeObject(data);
            var content = new StringContent(serializedData, Encoding.UTF8, "application/json");

            var query = $"{_changeAdvertKeywordUrl}?id={advertId}";
            var result = await _httpClient.PostAsync(query, content);
            if (result.StatusCode == HttpStatusCode.BadRequest)
                throw new ArgumentException($"Произошла ошибка при изменении ключевой фразы. {nameof(newKeyword)} = '{newKeyword}'");
            else if (result.StatusCode == HttpStatusCode.Unauthorized)
                throw new InvalidApiKeyException(_invalidApiKeyMessage);

            return result.IsSuccessStatusCode;
        }

        public async Task<bool> ChangeKeywordModeStatus(int advertId, bool enabled)
        {
            var query = $"{_changeAdvertKeywordUrl}?id={advertId}&fixed={enabled}";
            var result = await _httpClient.GetAsync(query);
            if (result.StatusCode == HttpStatusCode.BadRequest)
                throw new ArgumentException($"Произошла ошибка при изменении статуса режима ключевых фраз.");
            else if (result.StatusCode == HttpStatusCode.Unauthorized)
                throw new InvalidApiKeyException(_invalidApiKeyMessage);

            return result.IsSuccessStatusCode;
        }

        public async Task<bool> RenameAdvert(int advertId, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            var data = new
            {
                advertId,
                name
            };

            var serializedData = JsonConvert.SerializeObject(data);
            var content = new StringContent(serializedData, Encoding.UTF8, "application/json");

            var result = await _httpClient.PostAsync(_renameAdvertUrl, content);
            if (result.StatusCode == HttpStatusCode.BadRequest)
                throw new ArgumentException($"Неверный формат запроса для изменения названия РК. {nameof(advertId)}={advertId}; {nameof(name)}={name}");
            else if (result.StatusCode == HttpStatusCode.Unauthorized)
                throw new InvalidApiKeyException(_invalidApiKeyMessage);

            return result.IsSuccessStatusCode;
        }

        public async Task<bool> ChangeAdvertStatus(int advertId, AdvertStatus newStatus)
        {
            string? query;
            if (newStatus == AdvertStatus.InProgress)
                query = $"{_startAdvertUrl}?id={advertId}";
            else if (newStatus == AdvertStatus.Stopped)
                query = $"{_pauseAdvertUrl}?id={advertId}";
            else
                throw new ArgumentException($"Передан некорректный статус для рекламной кампании: '{newStatus}'");

            var result = await _httpClient.GetAsync(query);

            if (result.StatusCode == HttpStatusCode.BadRequest)
                throw new ArgumentException($"Передан некорректный ID рекламной кампании: '{advertId}'");
            else if (result.StatusCode == HttpStatusCode.Unauthorized)
                throw new InvalidApiKeyException(_invalidApiKeyMessage);

            return result.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Advert>> GetAdvertsAsync(AdvertStatus? status = null, AdvertType? type = null, int? count = null)
        {
            try
            {
                var advertCampaignsListQuery = GetAllAdvertsQuery(status, type, count);
                var campaigns = await GetAdvertsFromJson(advertCampaignsListQuery);
                await FillUpAdvertsInfo(campaigns);
                await FillUpAdvertsKeywords(campaigns);
                await FillUpAdvertsStatistics(campaigns);

                return campaigns;
            }
            catch (InvalidApiKeyException)
            {
                throw;
            }
        }

        public async Task<bool> ChangeCPM(Advert advertCampaign, int newCPM)
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

            var response = await _httpClient.PostAsync(_changeAdvertCPMUrl, content);

            if (response.StatusCode == HttpStatusCode.BadRequest)
                throw new ArgumentException("Некорректно переданы параметры для изменения CPM");
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new InvalidApiKeyException(_invalidApiKeyMessage);

            return response.IsSuccessStatusCode;
        }

        private static string GetAllAdvertsQuery(AdvertStatus? status, AdvertType? type, int? count)
        {
            var query = _getAllUrl;
            if (status is not null)
                query += $"status={(int)status}&";
            if (type is not null)
                query += $"type={(int)type}&";
            if (count is not null)
                query += $"limit={count}&";

            query += "order=change&direction=desc&";

            return query;
        }

        private async Task<IEnumerable<Advert>> GetAdvertsFromJson(string query)
        {
            var result = await _httpClient.GetAsync(query);

            if (result.StatusCode == HttpStatusCode.Unauthorized)
                throw new InvalidApiKeyException(_invalidApiKeyMessage);

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
                newCampaign.IsEnabled = newCampaign.Status == AdvertStatus.InProgress;
                newCampaign.IsAvailableToEnable = newCampaign.Status != AdvertStatus.Finished;

                advertCampaigns.Add(newCampaign);
            }

            return advertCampaigns;
        }

        private async Task FillUpAdvertsInfo(IEnumerable<Advert> campaigns)
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

        private async Task FillUpAdvertsKeywords(IEnumerable<Advert> campaigns)
        {
            var jObjects = await GetJObjectsByHttpQueries(campaigns, _getKeywordsUrl);
            foreach (var jObject in jObjects)
            {
                if (jObject is not null && jObject["words"].Value<JObject>().ContainsKey("pluse"))
                {
                    if (jObject["words"]["pluse"].HasValues && jObject["stat"].HasValues)
                    {
                        var findedCampaign = campaigns.First(campaign => campaign.AdvertId == jObject["stat"][0]["advertId"].Value<int>());
                        findedCampaign.Keyword = jObject["words"]["pluse"][0].Value<string>();
                    }
                }
            }
        }

        private async Task FillUpAdvertsStatistics(IEnumerable<Advert> campaigns)
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

        private async Task<List<JObject>> GetJObjectsByHttpQueries(IEnumerable<Advert> campaigns, string url)
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
                var result = await task;

                if (result.StatusCode == HttpStatusCode.Unauthorized)
                    throw new InvalidApiKeyException(_invalidApiKeyMessage);

                var stringResult = await result.Content.ReadAsStringAsync();
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
