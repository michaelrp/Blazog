using System.Text.Json.Serialization;

namespace Blazog.Models
{
    public class TagInfo
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("postCount")]
        public int PostCount { get; set; }
    }
}