using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Blazog.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Blazog.Services
{
    public class DataLoader : IDataLoader
    {
        private ILogger<DataLoader> logger;
        private HttpClient http;
        private IConfiguration configuration;
        private ILocalStorage store;
        private string rootUrl;

        public DataLoader(ILogger<DataLoader> logger,
            HttpClient http,
            IConfiguration configuration,
            ILocalStorage store)
        {
            this.logger = logger;
            this.http = http;
            this.configuration = configuration;
            this.store = store;
        }

        public async Task InitializeBlazogAppAsync()
        {
            logger.LogDebug("Initialize");

            rootUrl = configuration["RootUrl"];

            var existingLoadId = await store.GetLastLoadIdAsync();
            var remoteLoadId = await LoadLastLoadId(); // this updates the guid in the store

            logger.LogDebug($"existing {existingLoadId}, remote {remoteLoadId}");

            // if load id is different (new posts generated), reload
            if(string.IsNullOrEmpty(existingLoadId) || existingLoadId != remoteLoadId)
            {
                logger.LogDebug("Reload from remote");

                await LoadPostInfos();
                await LoadTagInfos();
                // await LoadOther();
            }
            else
            {
                logger.LogDebug("Load from local");

                // check to make sure not deleted
                var posts = await store.GetPostInfosAsync();
                if(posts == null)
                {
                    await LoadPostInfos();
                }

                // check to make sure not deleted
                var tags = await store.GetTagInfosAsync();
                if(tags == null)
                {
                    await LoadTagInfos();
                }
            }
        }

        public async Task<string> LoadLastLoadId()
        {
            logger.LogDebug("LoadGuid");

            var url = $"{rootUrl}/indexes/guid.txt";

            logger.LogDebug($"url {url}");
            
            var id = await http.GetStringAsync(url);

            logger.LogDebug($"id {id}");

            await store.SaveLastLoadIdAsync(id);

            return id;
        }        

        public async Task<PostDoc> LoadPost(string label, string hash)
        {
            logger.LogDebug($"LoadPost, label {label} hash {hash}");

            var url = $"{rootUrl}/posts/{label}.json?{hash}";
            var result = await http.GetStringAsync(url);
            var postDoc = JsonSerializer.Deserialize<PostDoc>(result);

            await store.SavePostDocAsync(label, postDoc);

            return postDoc;
        }

        private async Task LoadPostInfos()
        {
            logger.LogDebug("LoadPostInfos");

            var result = await http.GetStringAsync($"{rootUrl}/indexes/posts.json");
            var postInfos = JsonSerializer.Deserialize<IEnumerable<PostInfo>>(result);
            await store.SavePostInfosAsnyc(postInfos);
        }

        private async Task LoadTagInfos()
        {
            logger.LogDebug("LoadTaskInfos");

            var result = await http.GetStringAsync($"{rootUrl}/indexes/tags.json");
            var tagInfos = JsonSerializer.Deserialize<IEnumerable<TagInfo>>(result);
            await store.SaveTagInfosAsync(tagInfos);
        }

        private async Task LoadOther()
        {
            logger.LogDebug("LoadOther");

            var result = await http.GetStringAsync($"{rootUrl}/other/about.json");
            var postDoc = JsonSerializer.Deserialize<PostDoc>(result);
            await store.SaveOtherDocAsync("about", postDoc);
        }
    }
}