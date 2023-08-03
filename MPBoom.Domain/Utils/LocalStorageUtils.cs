using Microsoft.Extensions.Configuration;
using MPBoom.Domain.Interfaces;
using MPBoom.Domain.Models;
using Newtonsoft.Json;

namespace MPBoom.Domain.Utils
{
	public static class LocalStorageUtils
	{
		public static async Task<UserSettings?> GetUserSettings(this ILocalStorageService localStorage, IConfiguration configuration)
		{
			var userSettingsKey = configuration["LocalStorage:Keys:UserSettings"];
			var userSettingsJson = await localStorage.GetItem<string>(userSettingsKey);
			if (string.IsNullOrEmpty(userSettingsJson))
				return null;

			var settings = JsonConvert.DeserializeObject<UserSettings>(userSettingsJson);
			return settings;
		}
	}
}
