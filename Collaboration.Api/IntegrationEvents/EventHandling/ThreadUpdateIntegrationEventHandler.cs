using Collaboration.Api.IntegrationEvents.Events;
using Collaboration.Messaging.Models.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Collaboration.Api.IntegrationEvents.EventHandling
{
    public class ThreadUpdateIntegrationEventHandler : IIntegrationEventHandler<ThreadUpdateIntegrationEvent>
    {
        public ThreadUpdateIntegrationEventHandler()
        {
        }

        public async Task Handle(ThreadUpdateIntegrationEvent @event)
        {
            await Task.Run(() => Console.WriteLine("Publish received"));
        }
    }
}
