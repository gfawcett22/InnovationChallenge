using Collaboration.Messaging.Models;
using Collaboration.Messaging.Models.Abstractions;
using Collaboration.Messaging.RabbitMQ.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collaboration.Messaging.RabbitMQ
{
    public class RabbitMQEventBus : IEventBus, IDisposable
    {
        const string BROKER_NAME = "collaboration_event_bus";

        private IRabbitMQConnection _connection;
        private readonly IEventBusSubscriptionsManager _subsManager;
        IServiceProvider _sv;
        private IModel _consumerChannel;
        private string _queueName;

        public RabbitMQEventBus(IRabbitMQConnection connection, IEventBusSubscriptionsManager subsManager, string queueName = "", IServiceProvider sv = null)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _subsManager = subsManager ?? new InMemoryEventBusSubscriptionsManager();
            _queueName = queueName;
            _consumerChannel = CreateConsumerChannel();
            _sv = sv;
            _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
        }

        private void SubsManager_OnEventRemoved(object sender, string eventName)
        {
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            using (var channel = _connection.CreateModel())
            {
                channel.QueueUnbind(queue: _queueName,
                    exchange: BROKER_NAME,
                    routingKey: eventName);

                if (_subsManager.IsEmpty)
                {
                    _queueName = string.Empty;
                    _consumerChannel.Close();
                }
            }
        }


        public void Publish(IntegrationEvent @event)
        {
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            using (var channel = _connection.CreateModel())
            {
                var eventName = @event.GetType()
                    .Name;

                channel.ExchangeDeclare(exchange: BROKER_NAME,
                                    type: "direct");

                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: BROKER_NAME,
                                    routingKey: eventName,
                                    basicProperties: null,
                                    body: body);
            }
        }

        public void Subscribe<T, TH>()
             where T : IntegrationEvent
             where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();
            DoInternalSubscription(eventName);
            _subsManager.AddSubscription<T, TH>();
        }

        private void DoInternalSubscription(string eventName)
        {
            var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
            if (!containsKey)
            {
                if (!_connection.IsConnected)
                {
                    _connection.TryConnect();
                }

                using (var channel = _connection.CreateModel())
                {
                    channel.QueueBind(queue: _queueName,
                                      exchange: BROKER_NAME,
                                      routingKey: eventName);
                }
            }
        }

        public void Unsubscribe<T, TH>()
             where TH : IIntegrationEventHandler<T>
             where T : IntegrationEvent
        {
            _subsManager.RemoveSubscription<T, TH>();
        }

        public void Dispose()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }
            _subsManager.Clear();
        }

        private IModel CreateConsumerChannel()
        {
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            var channel = _connection.CreateModel();

            channel.ExchangeDeclare(exchange: BROKER_NAME,
                                 type: "direct");

            channel.QueueDeclare(queue: _queueName,
                                 durable: true,                                 
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);


            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var eventName = ea.RoutingKey;
                var message = Encoding.UTF8.GetString(ea.Body);

                await ProcessEvent(eventName, message);
            };

            channel.BasicConsume(queue: _queueName,
                                 autoAck: false,
                                 consumer: consumer);

            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
            };

            return channel;
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                foreach (var subscription in subscriptions)
                {
                    var eventType = _subsManager.GetEventTypeByName(eventName);
                    var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                    var handler = _sv.GetService(subscription.HandlerType);
                    var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                    await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                }
            }

        }
    }
}
