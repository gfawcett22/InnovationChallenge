using Collaboration.Messaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Collaboration.Api.IntegrationEvents.Events
{
    public class ThreadUpdateIntegrationEvent : IntegrationEvent
    {
        public int ThreadId { get; set; }
        public string Content { get; set; }

        public ThreadUpdateIntegrationEvent(int threadId, string content = "")
        {
            ThreadId = threadId;
            Content = content;
        }
    }
}
