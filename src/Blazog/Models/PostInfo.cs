using System;

namespace Blazog.Models
{
    public class PostInfo
    {
        public int Index { get; set; }
        public string Label { get; set; }
        public string[] Tags { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string Hash { get; set; }
    }
}