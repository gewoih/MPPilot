using MPBoom.App.Domain.Interfaces;
using MPBoom.App.Domain.Models;
using Newtonsoft.Json;

namespace MPBoom.App.Domain.Utils
{
    public static class LocalStorageUtils
    {
        public static async Task<UserSettings?> GetUserSettings(this ILocalStorageService localStorage, IConfiguration configuration)
        {
            var userSettingsKey = configuration.GetValue<string>("LocalStorage:Keys:UserSettings");
            var userSettingsJson = await localStorage.GetItem<string>(userSettingsKey);
            if (string.IsNullOrEmpty(userSettingsJson))
                return null;

            var settings = JsonConvert.DeserializeObject<UserSettings>(userSettingsJson);
            return settings;
        }
    }
}
