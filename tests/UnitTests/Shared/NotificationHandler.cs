using Medianiz.Shared;
using Mediator.Interfaces;

namespace Medianiz.Tests.Shared
{
    public class NotificationHandler : INotificationHandler<NotificationEvent>
    {
        private readonly Counter _counter;
        public NotificationHandler(Counter counter) => _counter = counter;

        public Task Handle(NotificationEvent notification, CancellationToken cancellationToken)
        {
            _counter.Count++;
            return Task.CompletedTask;
        }
    }
}