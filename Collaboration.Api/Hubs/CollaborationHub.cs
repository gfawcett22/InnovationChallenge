using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Collaboration.Api.Hubs
{
    public class CollaborationHub : Hub
    {
        ICollaborationService _collaborationService = null;

        CollaborationHub(ICollaborationService collaborationService)
        {
            _collaborationService = collaborationService;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.InvokeAsync("Threads", $"{Context.ConnectionId} joined");
        }

        public Task Send(string message)
        {
            return Clients.All.InvokeAsync("Threads", $"{Context.ConnectionId}: {message}");
        }

        public Task GetThreads(string docId)
        {
            
            return this.Groups.AddAsync(this.Context.ConnectionId, docId);
            //return null Collaboration.Threads.GetThreads(docId);
        }

        public Task SendThread(long docId, object thread)
        {
            return _collaborationService.PushOutNewThread(docId, thread);
            //Clients.All.InvokeAsync("Threads", "testing");
            //return Clients.Group(docId.ToString()).InvokeAsync("Threads", "groupTest");
        }


    }
}
