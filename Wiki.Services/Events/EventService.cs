using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using Wiki.Data.Domain.Events.Commands;
using Wiki.Data.Domain.Events.Queries;
using Wiki.Entities;
using Wiki.Entities.Common;
using Wiki.Entities.Domain;
using Wiki.Entities.Events;

namespace Wiki.Services.Events
{
    public class EventService : IEventService
    {
        private readonly IMediator _mediator;

        public EventService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task CreateEntityEventAsync(Entity entity)
        {
            //Get latest event:
            var getLatestEntityEventQuery = new GetLatestEntityEvent.Query {AggregateId = entity.Id};
            var latestEvent = await _mediator.SendAsync(getLatestEntityEventQuery);

            //Create current event
            var currentEvent = new Event
            {
                AggregateId = entity.Id,
                Timestamp = DateTime.UtcNow,
                Version = latestEvent == null ? Config.FirstVersion : ++latestEvent.Version,
                Data = JsonConvert.SerializeObject(entity)
            };

            //Save event
            var command = new Insert.Command {Event = currentEvent};
            await _mediator.SendAsync(command);
        }

        public Task<IEnumerable<Event>> GetEventsByAggregateIdAsync(int aggregateId)
        {
            var query = new GetByAggregateId.Query {AggregateId = aggregateId};
            return _mediator.SendAsync(query);
        }

        public Page GetPageFromEvent(Event currentEvent)
        {
            return string.IsNullOrWhiteSpace(currentEvent?.Data)
                ? null
                : JsonConvert.DeserializeObject<Page>(currentEvent.Data);
        }
    }
}
