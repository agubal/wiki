using System.Threading.Tasks;
using MediatR;
using Wiki.Entities.Events;

namespace Wiki.Data.Domain.Events.Commands
{
    public class Insert
    {
        public class Command : IAsyncRequest
        {
            public Event Event { get; set; }
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
                    "INSERT INTO Events (AggregateId, Version, Timestamp, Data) " +
                    "VALUES (@AggregateId, @Version, @Timestamp, @Data)",
                        new { command.Event.AggregateId, command.Event.Version, command.Event.Timestamp, command.Event.Data });
            }
        }
    }
}
