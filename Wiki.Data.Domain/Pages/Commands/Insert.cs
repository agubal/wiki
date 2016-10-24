using System;
using System.Threading.Tasks;
using MediatR;
using Wiki.Entities.Domain;

namespace Wiki.Data.Domain.Pages.Commands
{
    public class Insert
    {
        public class Command : IAsyncRequest
        {
            public Page Page { get; set; }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly IRepository _repository;

            public Handler(IRepository repository)
            {
                _repository = repository;
            }

            protected override async Task HandleCore(Command command)
            {
                await _repository.ExecuteAsync(
                    "INSERT INTO Pages (Title, Text, Created) VALUES (@Title, @Text, @Created)", 
                    new { command.Page.Title, command.Page.Text, Created = DateTime.UtcNow });
            }
        }
    }
}
