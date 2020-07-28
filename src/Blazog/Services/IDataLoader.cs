using System.Threading.Tasks;
using Blazog.Models;

namespace Blazog.Services
{
    public interface IDataLoader
    {
        Task InitializeBlazogAppAsync();
        Task<PostDoc> LoadPost(string label, string hash);
    }
}