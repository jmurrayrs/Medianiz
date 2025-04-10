using Medianiz.Shared;
using Mediator.Interfaces;

namespace Medianiz.Tests.Shared
{
    public class AnotherNotificationHandler : INotificationHandler<NotificationEvent>
    {
        private readonly Counter _counter;
        public AnotherNotificationHandler(Counter counter) => _counter = counter;

        public Task Handle(NotificationEvent notification, CancellationToken ct)
        {
            _counter.Count++;
            return Task.CompletedTask;
        }
    }
}