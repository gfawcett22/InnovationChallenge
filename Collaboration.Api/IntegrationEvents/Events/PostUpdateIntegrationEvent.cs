using Collaboration.Messaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Collaboration.Api.IntegrationEvents.Events
{
    public class PostUpdateIntegrationEvent : IntegrationEvent
    {
        public int ThreadId { get; set; }


        public PostUpdateIntegrationEvent(int threadId)
        {
            ThreadId = threadId;

        }
    }
}
