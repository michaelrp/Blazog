using System;
using System.Text;

namespace Blazog.Models
{
    public class PostDoc
    {
        private string mainHtml;
        private string sideHtml;

        public string Label { get; set; }
        public string MainContent { get; set; }
        public string SideContent { get; set; }

        public string GetMainContentHtml()
        {
            if(mainHtml == null)
            {
                mainHtml = DecodeBase64(MainContent);
            }
            return mainHtml;
        }

        public string GetSideContentHtml()
        {
            if(sideHtml == null)
            {
                return sideHtml = DecodeBase64(SideContent);
            }
            return sideHtml;
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