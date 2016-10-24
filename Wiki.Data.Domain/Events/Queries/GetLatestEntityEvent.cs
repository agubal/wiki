using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Wiki.Entities.Events;

namespace Wiki.Data.Domain.Events.Queries
{
    public class GetLatestEntityEvent : IAsyncRequest<Event>
    {

        public class Query : IAsyncRequest<Event>
        {
            public int AggregateId { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Query, Event>
        {
            private readonly IRepository _repository;

            public Handler(IRepository repository)
            {
                _repository = repository;
            }

            public async Task<Event> Handle(Query query)
            {
                return (await _repository.QueryAsync<Event>(
                    "SELECT TOP 1 * FROM Events " +
                    "WHERE AggregateId = @AggregateId " +
                    "ORDER BY Version DESC", new {query.AggregateId})).FirstOrDefault();
            }
        }
    }
}
