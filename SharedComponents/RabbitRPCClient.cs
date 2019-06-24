using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SharedComponents
{
    public class RabbitRPCClient : IRPCClient
    {
        private readonly IModel channel;
        private readonly string correlationId;
        private readonly string replyQueueName;
        private readonly IBasicProperties props;
        private readonly EventingBasicConsumer consumer;
        private readonly BlockingCollection<string> responseQueue;
        public RabbitRPCClient(string hostname)
        {
            if (string.IsNullOrWhiteSpace(hostname))
            {
                throw new ArgumentNullException(nameof(hostname));
            }

            var factory = new ConnectionFactory() { HostName = hostname };
            var connection = factory.CreateConnection();
            channel = connection.CreateModel();

            replyQueueName = channel.QueueDeclare().QueueName;
            consumer = new EventingBasicConsumer(channel);

            correlationId = Guid.NewGuid().ToString();
            props = channel.CreateBasicProperties();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;

            responseQueue = new BlockingCollection<string>();

            consumer.Received += Consumer_Received;
        }

        public string SendMessage(string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("", CommonConstants.QUEUE_NAME, props, messageBytes);
            channel.BasicConsume(consumer, replyQueueName, true);

            return responseQueue.Take();
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            if (e.BasicProperties.CorrelationId == correlationId)
            {
                var response = Encoding.UTF8.GetString(e.Body);
                responseQueue.Add(response);
            }
        }
    }
}
