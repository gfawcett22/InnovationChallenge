using Collaboration.Core.Models;
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
        public int DocumentId { get; set; }
        public string Title { get; set; }
        public IEnumerable<Post> Posts { get; set; }

        public ThreadUpdateIntegrationEvent(int threadId, int documentId, string title, IEnumerable<Post> posts)
        {
            ThreadId = threadId;
            DocumentId = documentId;
            Title = title;
            Posts = posts;
        }
    }
}
