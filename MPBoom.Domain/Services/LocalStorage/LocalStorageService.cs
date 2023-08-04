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
            try
            {
                return await jsRuntime.InvokeAsync<T>("localStorage.getItem", key);
            }
            catch (JSDisconnectedException)
            {
                return default;
            }
        }

        public async Task SetItemAsync<T>(string key, T value)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
            }
            catch (JSDisconnectedException)
            {

            }
        }

        public async Task RemoveItemAsync(string key)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
            }
            catch (JSDisconnectedException)
            {

            }
        }
    }
}
