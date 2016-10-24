using System;
using Wiki.Entities.Domain;

namespace Wiki.Entities.Models
{
    public class PageVersionModel
    {
        public int Version { get; set; }
        public DateTime Timestamp { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }

        public PageVersionModel(int version, DateTime timestamp, string text, string title)
        {
            Version = version;
            Timestamp = timestamp;
            Text = text;
            Title = title;
        }

        public PageVersionModel(Page page) : this(default(int), page.Created, page.Text, page.Title)
        {
        }
    }
}
