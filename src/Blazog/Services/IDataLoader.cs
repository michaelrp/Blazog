using System.Threading.Tasks;
using Blazog.Models;

namespace Blazog.Services
{
    public interface IDataLoader
    {
        Task InitializeBlazogAppAsync();
    }
}