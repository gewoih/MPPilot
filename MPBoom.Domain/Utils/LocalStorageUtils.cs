using Microsoft.Extensions.Configuration;
using MPBoom.Domain.Models;
using MPBoom.Domain.Services.LocalStorage;
using Newtonsoft.Json;

namespace MPBoom.Domain.Utils
{
    public static class LocalStorageUtils
	{
		public static async Task<UserSettings?> GetUserSettings(this ILocalStorageService localStorage, IConfiguration configuration)
		{
			var userSettingsKey = configuration["LocalStorage:Keys:UserSettings"];
			var userSettingsJson = await localStorage.GetItemAsync<string>(userSettingsKey);
			if (string.IsNullOrEmpty(userSettingsJson))
				return null;

			var settings = JsonConvert.DeserializeObject<UserSettings>(userSettingsJson);
			return settings;
		}
	}
}
