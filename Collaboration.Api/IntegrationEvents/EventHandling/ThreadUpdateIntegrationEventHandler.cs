using Collaboration.Api.IntegrationEvents.Events;
using Collaboration.Messaging.Models.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Collaboration.Api.IntegrationEvents.EventHandling
{
    public class ThreadUpdateIntegrationEventHandler : IIntegrationEventHandler<ThreadUpdateIntegrationEvent>
    {
        private ILogger _logger;
        public ThreadUpdateIntegrationEventHandler(ILogger logger)
        {
            _logger = logger;
        }

        public async Task Handle(ThreadUpdateIntegrationEvent @event)
        {
            await Task.Run(() => _logger.LogDebug("Publish received"));
        }
    }
}
