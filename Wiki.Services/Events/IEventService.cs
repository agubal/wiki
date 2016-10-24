using System.Collections.Generic;
using System.Threading.Tasks;
using Wiki.Entities;
using Wiki.Entities.Domain;
using Wiki.Entities.Events;

namespace Wiki.Services.Events
{
    public interface IEventService
    {
        Task CreateEntityEventAsync(Entity entity);
        Task<IEnumerable<Event>> GetEventsByAggregateIdAsync(int aggregateId);
        Page GetPageFromEvent(Event currentEvent);
    }
}
