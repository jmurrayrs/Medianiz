using Medianiz.Shared;
using Medianiz.Tests.Shared;
using Mediator.Extensions;
using Mediator.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Medianiz.Tests.UnitTests
{

    public class PublishTests
    {
        [Fact]
        public async Task Publish_Notification_Triggers_Multiple_Handlers()
        {
            var services = new ServiceCollection();
            var counter = new Counter();

            services.AddSingleton(counter);
            services.AddMedianiz(typeof(NotificationHandler));

            var provider = services.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMedianiz>();

            await mediator.Publish(new NotificationEvent());


            Assert.Equal(2, counter.Count);
        }
    }
}