using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Wiki.Entities.Domain;

namespace Wiki.Data.Domain.Pages.Queries
{
    public class GetAll : IAsyncRequest<IEnumerable<Page>>
    {

        public class Query : IAsyncRequest<IEnumerable<Page>>
        {
        }

        public class Handler : IAsyncRequestHandler<Query, IEnumerable<Page>>
        {
            private readonly IRepository _repository;

            public Handler(IRepository repository)
            {
                _repository = repository;
            }

            public async Task<IEnumerable<Page>> Handle(Query query)
            {
                return await _repository.QueryAsync<Page>("SELECT * FROM Pages");
            }
        }
    }
}
