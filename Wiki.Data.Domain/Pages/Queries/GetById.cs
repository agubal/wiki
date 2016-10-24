using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Wiki.Entities.Domain;

namespace Wiki.Data.Domain.Pages.Queries
{
    public class GetById: IAsyncRequest<Page>
    {
        public class Query : IAsyncRequest<Page>
        {
            public int Id { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Query, Page>
        {
            private readonly IRepository _repository;

            public Handler(IRepository repository)
            {
                _repository = repository;
            }

            public async Task<Page> Handle(Query query)
            {
                return (await _repository.QueryAsync<Page>(
                    "SELECT * FROM Pages WHERE id = @id", 
                    new { query.Id })).FirstOrDefault();
            }
        }
    }
}
