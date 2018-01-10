using Collaboration.Api.Hubs;
using Collaboration.Api.IntegrationEvents.Events;
using Collaboration.Messaging.Models.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Collaboration.Api.IntegrationEvents.EventHandling
{
    public class ThreadUpdateIntegrationEventHandler : IIntegrationEventHandler<ThreadUpdateIntegrationEvent>
    {
        ICollaborationService _collaborationService;

        public ThreadUpdateIntegrationEventHandler(ICollaborationService collaborationService)
        {
            _collaborationService = collaborationService;
        }

        public async Task Handle(ThreadUpdateIntegrationEvent @event)
        {
            await Task.Run(() => Console.WriteLine("ThreadUpdate event"));
            //await _collaborationService.PushOutNewThreadsForDocument(@event.DocumentId);
        }
    }
}
