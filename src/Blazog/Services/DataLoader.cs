using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Blazog.Models;
using Microsoft.Extensions.Logging;

namespace Blazog.Services
{
    public class DataLoader : IDataLoader
    {
        private ILogger<DataLoader> logger;
        private HttpClient http;
        private ILocalStorage store;

        public DataLoader(ILogger<DataLoader> logger,
            HttpClient http, 
            ILocalStorage store)
        {
            this.logger = logger;
            this.http = http;
            this.store = store;
        }

        public async Task InitializeBlazogAppAsync()
        {
            logger.LogDebug("Initialize");

            var appConfig = await LoadAppConfig();
            await LoadPostInfos(appConfig.RootUrl);
            await LoadTagInfos(appConfig.RootUrl);
            // await LoadOther(appConfig.RootUrl);
        }

        private async Task<AppConfig> LoadAppConfig()
        {
            logger.LogDebug("LoadAppConfig");

            var result = await http.GetStringAsync("config.json");
            var appConfig = JsonSerializer.Deserialize<AppConfig>(result);

            await store.SaveAppConfig(appConfig);
            
            return appConfig;
        }

        private async Task LoadPostInfos(string rootUrl)
        {
            logger.LogDebug("LoadPostInfos");

            var result = await http.GetStringAsync($"{rootUrl}/posts/posts.json");
            var postInfos = JsonSerializer.Deserialize<IEnumerable<PostInfo>>(result);
            await store.SavePostInfosAsnyc(postInfos);
        }

        private async Task LoadTagInfos(string rootUrl)
        {
            logger.LogDebug("LoadTaskInfos");

            var result = await http.GetStringAsync($"{rootUrl}/tags/tags.json");
            var tagInfos = JsonSerializer.Deserialize<IEnumerable<TagInfo>>(result);
            await store.SaveTagInfosAsync(tagInfos);
        }

        private async Task LoadOther(string rootUrl)
        {
            logger.LogDebug("LoadOther");

            var result = await http.GetStringAsync($"{rootUrl}/other/about.json");
            var postDoc = JsonSerializer.Deserialize<PostDoc>(result);
            await store.SaveOtherDocAsync("about", postDoc);
        }
    }
}