using Collaboration.Api.Hubs;
using Collaboration.Api.IntegrationEvents.Events;
using Collaboration.Messaging.Models.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Collaboration.Api.IntegrationEvents.EventHandling
{
    public class PostUpdateIntegrationEventHandler : IIntegrationEventHandler<PostUpdateIntegrationEvent>
    {
        private ICollaborationService _collaborationService;
        public PostUpdateIntegrationEventHandler(ICollaborationService collaborationService)
        {
            _collaborationService = collaborationService;
        }

        public async Task Handle(PostUpdateIntegrationEvent @event)
        {
            await _collaborationService.PushOutPostsForThread(@event.ThreadId);
        }
    }
}
