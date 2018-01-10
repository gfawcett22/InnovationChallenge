using Collaboration.Api.Hubs;
using Collaboration.Api.IntegrationEvents.Events;
using Collaboration.Messaging.Models.Abstractions;
using System.Threading.Tasks;
using Collaboration.Core.Data;

namespace Collaboration.Api.IntegrationEvents.EventHandling
{
    public class PostUpdateIntegrationEventHandler : IIntegrationEventHandler<PostUpdateIntegrationEvent>
    {
        private ICollaborationService _collaborationService;
        
        public PostUpdateIntegrationEventHandler(ICollaborationService collaborationService, IThreadRepository threadRepository, IPostRepository postRepository)
        {
            _collaborationService = collaborationService;
        }

        public async Task Handle(PostUpdateIntegrationEvent @event)
        {
            await _collaborationService.PushOutPostsForThread(@event.ThreadId);
        }
    }
}
