using Microsoft.JSInterop;
using MPBoom.App.Domain.Interfaces;

namespace MPBoom.App.Domain.Services
{
    public class LocalStorageService : ILocalStorageService
    {
        private readonly IJSRuntime jsRuntime;

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public async Task<T> GetItem<T>(string key)
        {
            return await jsRuntime.InvokeAsync<T>("localStorage.getItem", key);
        }

        public async Task SetItem<T>(string key, T value)
        {
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
        }

        public async Task RemoveItem(string key)
        {
            await jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
    }
}
