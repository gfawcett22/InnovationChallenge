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
        public int DocumentId { get; set; }

        public ThreadUpdateIntegrationEvent(int documentId)
        {
            DocumentId = documentId;
        }
    }
}
