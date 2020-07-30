using System.Text.Json.Serialization;

namespace Blazog.Models
{
    public class AppConfig
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("rootUrl")]
        public string RootUrl { get; set; }
        [JsonPropertyName("copyrightText")]
        public string CopyrightText { get; set; }
        [JsonPropertyName("copyrightUrl")]
        public string CopyrightUrl { get; set; }
    }
}