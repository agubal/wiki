using System;
using System.Collections.Generic;
using System.Linq;
using Wiki.Entities.Events;

namespace Wiki.Entities
{
    public abstract class State<T> where T : Entity
    {
        public T Entity { get; set; }
        private readonly List<Event> _events;

        protected State(T entity, IEnumerable<Event> events, int? version = null)
        {
            if(entity == null) throw new ArgumentNullException(nameof(entity));
            Entity = entity;
            _events = events?.ToList() ?? new List<Event>();
            AggregateEvents(version);
        }

        protected abstract void Mutate(Event currentEvent);

        private void AggregateEvents(int? version = null)
        {
            if(version.HasValue && _events.All(e => e.Version != version.Value))
                throw new ApplicationException($"Version {version.Value} is invalid");
            if (!_events.Any()) return;
            foreach (var currentEvent in _events)
            {
                Mutate(currentEvent);
                if(version.HasValue && currentEvent.Version == version.Value) break;
            }
        }
    }
}
