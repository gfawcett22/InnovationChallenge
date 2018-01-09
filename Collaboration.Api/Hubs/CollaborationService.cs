using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Collaboration.Api.Hubs
{
    public class CollaborationService
    {
        private readonly IHubContext<CollaborationHub> _hubContext;
        public CollaborationService(IHubContext<CollaborationHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public Task PushOutNewThread(long docId, object thread)
        {
            return _hubContext.Clients.Group(docId.ToString()).InvokeAsync("Threads", thread);
        }
    }

    public interface ICollaborationService
    {
        Task PushOutNewThread(long docId, object thread);
    }
}
