using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Wiki.Entities.Events;

namespace Wiki.Data.Domain.Events.Queries
{
    public class GetByAggregateId : IAsyncRequest<IEnumerable<Event>>
    {

        public class Query : IAsyncRequest<IEnumerable<Event>>
        {
            public int AggregateId { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Query, IEnumerable<Event>>
        {
            private readonly IRepository _repository;

            public Handler(IRepository repository)
            {
                _repository = repository;
            }

            public async Task<IEnumerable<Event>> Handle(Query query)
            {
                return await _repository.QueryAsync<Event>(
                    "SELECT * FROM Events " +
                    "WHERE AggregateId = @AggregateId ORDER BY Version", 
                    new { query.AggregateId});
            }
        }
    }
}
