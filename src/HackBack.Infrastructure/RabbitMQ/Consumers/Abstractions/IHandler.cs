using RabbitMQ.Client.Events;
using ResultSharp.Core;

namespace ZazaTemplate.Infrastructure.RabbitMQ.Consumers.Abstractions
{
    public interface IHandler
    {
        public Task<Result> HandleAsync(BasicDeliverEventArgs args, CancellationToken cancellationToken);
    }
}
