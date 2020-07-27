namespace Blazog.Models
{
    public class PostInfo
    {
        public string Title { get; set; }
        public string Label { get; set; }
        public string Blurb { get; set; }
        public string[] Tags { get; set; }
        public string PreviousTitle { get; set; }
        public string PreviousLabel { get; set; }
        public string NextTitle { get; set; }
        public string NextLabel { get; set; }
    }
}