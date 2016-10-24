using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Wiki.Entities.Events;

namespace Wiki.Data.Domain.Events.Queries
{
    public class GetById : IAsyncRequest<Event>
    {
        public class Query : IAsyncRequest<Event>
        {
            public int Id { get; set; }
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
                    "SELECT * FROM Events " +
                    "WHERE Id = @Id ORDER BY Version", 
                    new { query.Id})).FirstOrDefault();
            }
        }
    }
}
