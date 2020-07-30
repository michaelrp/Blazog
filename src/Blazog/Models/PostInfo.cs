using System.Text.Json.Serialization;

namespace Blazog.Models
{
    public class PostInfo
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }
        [JsonPropertyName("label")]
        public string Label { get; set; }
        [JsonPropertyName("tags")]
        public string[] Tags { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("date")]
        public string Date { get; set; }
        [JsonPropertyName("blurb")]
        public string Blurb { get; set; }
        [JsonPropertyName("hash")]
        public string Hash { get; set; }
    }
}