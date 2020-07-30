using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace Blazog.Models
{
    public class PostDoc
    {
        private string html;

        [JsonPropertyName("index")]
        private int Index { get; set; }
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
        [JsonPropertyName("content")]
        public string Content { get; set; }
        [JsonPropertyName("hash")]
        public string Hash { get; set; }

        public string GetContentHtml()
        {
            if (html == null)
            {
                html = DecodeBase64(Content);
            }
            return html;
        }

        private string DecodeBase64(string base64Content)
        {
            if (string.IsNullOrEmpty(base64Content))
            {
                return "";
            }

            var base64EncodedBytes = Convert.FromBase64String(base64Content);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}