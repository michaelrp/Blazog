using System;
using System.Text;

namespace Blazog.Models
{
    public class PostDoc
    {
        private string html;

        public string Label { get; set; }
        public string Tags { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }

        public string GetContentHtml()
        {
            if(html == null)
            {
                html = DecodeBase64(Content);
            }
            return html;
        }

        private string DecodeBase64(string base64Content)
        {
            if(string.IsNullOrEmpty(base64Content))
            {
                return "";
            }

            var base64EncodedBytes = Convert.FromBase64String(base64Content);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}