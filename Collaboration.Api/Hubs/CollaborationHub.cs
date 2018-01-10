using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Collaboration.Api.Hubs
{
    public class CollaborationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            //await Clients.All.InvokeAsync("Threads", $"{Context.ConnectionId} joined");
        }

        public Task Send(string message)
        {
            return Clients.All.InvokeAsync("Threads", $"{Context.ConnectionId}: {message}");
        }


        //public Task SendThread(long docId)
        //{
        //    //return _collaborationService.PushOutNewThread(docId);
        //    //Clients.All.InvokeAsync("Threads", "testing");
        //    //return Clients.Group(docId.ToString()).InvokeAsync("Threads", "groupTest");
        //}


    }
}
