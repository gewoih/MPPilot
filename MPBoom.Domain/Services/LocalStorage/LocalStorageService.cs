using Microsoft.JSInterop;

namespace MPBoom.Domain.Services.LocalStorage
{
    public class LocalStorageService : ILocalStorageService
    {
        private readonly IJSRuntime jsRuntime;

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public async Task<T> GetItemAsync<T>(string key)
        {
            return await jsRuntime.InvokeAsync<T>("localStorage.getItem", key);
        }

        public async Task SetItemAsync<T>(string key, T value)
        {
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
        }

        public async Task RemoveItemAsync(string key)
        {
            await jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
    }
}
