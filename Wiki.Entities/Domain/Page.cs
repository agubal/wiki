using System;

namespace Wiki.Entities.Domain
{
    public class Page : Entity
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
    }
}
