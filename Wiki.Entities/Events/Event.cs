using System;

namespace Wiki.Entities.Events
{
    public class Event : Entity
    {
        public int AggregateId { get; set; }
        public int Version { get; set; }  
        public DateTime Timestamp { get; set; }
        public string Data { get; set; }
    }
}
