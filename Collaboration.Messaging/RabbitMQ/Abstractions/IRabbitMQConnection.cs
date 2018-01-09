using RabbitMQ.Client;
using System;

namespace Collaboration.Messaging.RabbitMQ.Abstractions
{
    public interface IRabbitMQConnection : IDisposable
    {
        bool IsConnected { get; }
        bool TryConnect();
        IModel CreateModel();
    }
}
