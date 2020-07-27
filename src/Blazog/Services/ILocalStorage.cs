using System.Collections.Generic;
using System.Threading.Tasks;
using Blazog.Models;

namespace Blazog.Services
{
    public interface ILocalStorage
    {
        Task ClearAllDataAsync();
        Task<AppConfig> GetAppConfigAsync();
        Task<IEnumerable<PostInfo>> GetPostInfosAsync();
        Task<PostInfo> GetPostInfoAsync(string label);
        Task<PostDoc> GetPostDocAsync(string label);
        Task<IEnumerable<TagInfo>> GetTagInfosAsync();
        Task<PostDoc> GetOtherDocAsync(string label);
        Task<string> GetLastLoadIdAsync();
        Task SaveAppConfig(AppConfig appConfig);
        Task SavePostInfosAsnyc(IEnumerable<PostInfo> postInfos);
        Task SaveTagInfosAsync(IEnumerable<TagInfo> tagInfos);
        Task SavePostDocAsync(string label, PostDoc postDoc);
        Task SaveOtherDocAsync(string label, PostDoc postDoc);
        Task SaveLastLoadIdAsync(string id);
    }
}