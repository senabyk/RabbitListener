using RabbitListener.Application.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitListener.Infrastructure.Messaging
{
    public class RabbitListener
    {
        private readonly IUrlProcessor _urlProcessor;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitListener(IUrlProcessor urlProcessor)
        {
            _urlProcessor = urlProcessor;

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare("urls", false, false, false, null);
        }

        public void StartListening()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var url = Encoding.UTF8.GetString(body);

                await _urlProcessor.ProcessUrlAsync(url);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume("urls", false, consumer);
        }
    }
}
