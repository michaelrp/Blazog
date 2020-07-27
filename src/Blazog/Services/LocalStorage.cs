using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Blazog.Models;

namespace Blazog.Services
{
    public class LocalStorage : ILocalStorage
    {
        private readonly IJSRuntime jsRuntime;

        public LocalStorage(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public async Task ClearAllDataAsync()
        {
            await jsRuntime.InvokeAsync<object>("localStorage.clear");
        }

        public async Task<AppConfig> GetAppConfigAsync()
            => await GetValueAsync<AppConfig>("config");
        public async Task<IEnumerable<PostInfo>> GetPostInfosAsync()
            => await GetValueAsync<IEnumerable<PostInfo>>("posts");

        public async Task<PostInfo> GetPostInfoAsync(string label)
            => (await GetPostInfosAsync()).FirstOrDefault(p => p.Label == label);

        public async Task<PostDoc> GetPostDocAsync(string label)
            => await GetValueAsync<PostDoc>($"posts:{label}");

        public async Task<IEnumerable<TagInfo>> GetTagInfosAsync()
            => await GetValueAsync<IEnumerable<TagInfo>>("tags");

        public async Task<PostDoc> GetOtherDocAsync(string label)
            => await GetValueAsync<PostDoc>($"other:{label}");

        public async Task<string> GetLastLoadIdAsync()
            => await GetValueAsync<string>("last-load");

        public async Task SaveAppConfig(AppConfig appConfig)
            => await SetValueAsync("config", appConfig);
        public async Task SavePostInfosAsnyc(IEnumerable<PostInfo> postInfos)
            => await SetValueAsync<IEnumerable<PostInfo>>("posts", postInfos);

        public async Task SaveTagInfosAsync(IEnumerable<TagInfo> tagInfos)
            => await SetValueAsync<IEnumerable<TagInfo>>("tags", tagInfos);

        public async Task SavePostDocAsync(string label, PostDoc postDoc)
            => await SetValueAsync<PostDoc>($"posts:{label}", postDoc);
        
        public async Task SaveOtherDocAsync(string label, PostDoc postDoc)
            => await SetValueAsync<PostDoc>($"other:{label}", postDoc);
        
        public async Task SaveLastLoadIdAsync(string id)
            => await SetValueAsync<string>("last-load", id);

        private async Task<T> GetValueAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Must not be null or empty");
            }

            var storageValue = await jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
            if (storageValue != null)
            {
                return JsonSerializer.Deserialize<T>(storageValue);
            }
            else
            {
                return default(T);
            }
        }

        private async Task SetValueAsync<T>(string key, T value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Must not be null or empty");
            }

            if (value == null)
            {
                await jsRuntime.InvokeAsync<object>("localStorage.removeItem", key);
            }
            else
            {
                var storageValue = JsonSerializer.Serialize(value);
                await jsRuntime.InvokeAsync<object>("localStorage.setItem", key, storageValue);
            }
        }
    }
}