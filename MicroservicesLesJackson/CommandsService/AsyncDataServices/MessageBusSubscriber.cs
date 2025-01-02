using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommandsService.EventProcessing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandsService.AsyncDataServices
{
    // This class is responsible for listening to the message bus and processing the messages
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        public MessageBusSubscriber(
            IConfiguration configuration,
            IEventProcessor eventProcessor)
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;

            InitializeRabbitMQ();
        }

        private async Task InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory() { HostName = _configuration["RabbitMQHost"], Port = int.Parse(_configuration["RabbitMQPort"]) };

            _connection = await factory.CreateConnectionAsync();
            // _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _queueName,
                exchange: "trigger",
                routingKey: "");

            Console.WriteLine("--> Listenting on the Message Bus...");

            //_connection.ConnectionShutdown += RabbitMQ_ConnectionShitdown;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ModuleHandle, ea) =>
            {
                Console.WriteLine("--> Event Received!");

                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                _eventProcessor.ProcessEvent(notificationMessage);
            };

            // the next line is the one that actually starts the listening
            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> Connection Shutdown");
        }

        public override void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }

            base.Dispose();
        }
    }
}