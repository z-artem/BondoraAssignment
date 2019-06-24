using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedComponents
{
    public class RabbitRPCServer : IRPCServer
    {
        private Func<string, string> receiverCallback;
        private IModel channel;

        public RabbitRPCServer(string hostname)
        {
            if (string.IsNullOrWhiteSpace(hostname))
            {
                throw new ArgumentNullException(nameof(hostname));
            }

            Init(hostname);
        }

        public void RegisterReceiverCallback(Func<string, string> callback)
        {
            receiverCallback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        private void Init(string hostname)
        {
            var factory = new ConnectionFactory() { HostName = hostname };
            var connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(CommonConstants.QUEUE_NAME, false, false, false, null);
            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(CommonConstants.QUEUE_NAME, false, consumer);

            consumer.Received += Consumer_Received;
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body);
            var callbackResponse = receiverCallback(message);

            var replyProperties = channel.CreateBasicProperties();
            replyProperties.CorrelationId = e.BasicProperties.CorrelationId;
            channel.BasicPublish("", e.BasicProperties.ReplyTo, replyProperties, Encoding.UTF8.GetBytes(callbackResponse));
            channel.BasicAck(e.DeliveryTag, false);
        }
    }
}
