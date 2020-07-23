using System.Collections.Generic;
using System.Threading.Tasks;
using Blazog.Models;

namespace Blazog.Services
{
    public interface IPostService
    {
        Task<IEnumerable<PostInfo>> GetPostsAsync(int page);
        Task<string> GetPostAsync(string label);
        Task<IEnumerable<TagInfo>> GetTagsAsync();
        Task<IEnumerable<PostInfo>> GetTagAsync(string tag);   
    }
}