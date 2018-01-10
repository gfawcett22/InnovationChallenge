using Collaboration.Messaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Collaboration.Api.IntegrationEvents.Events
{
    public class PostUpdateIntegrationEvent : IntegrationEvent
    {
        public int PostId;
        public int ThreadId { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public DateTime TimeStamp { get; set; }

        public PostUpdateIntegrationEvent(int postId, int threadId, string content, string username, DateTime timeStamp)
        {
            PostId = postId;
            ThreadId = threadId;
            Content = content;
            UserName = username;
            TimeStamp = timeStamp;
        }
    }
}
