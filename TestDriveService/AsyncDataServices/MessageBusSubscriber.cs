using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using TestDriveService.EventProcessing;

namespace TestDriveService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;
        private string _exchange;
        private string _routingKey;
        private string _queue;

        public MessageBusSubscriber(
            IConfiguration configuration,
            IEventProcessor eventProcessor)
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;

            InitializeRabbitMQ();
        }

        private void InitializeRabbitMQ()
        {
//            _exchange = _configuration["RabbitMQExchange"];
            _exchange = _configuration["RabbitMQExchange2"];
            _routingKey = _configuration["RabbitMQRoutingKey"];
            _queue = _configuration["RabbitMQQueue"];
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"]),
                UserName = _configuration["RabbitMQUserName"],
                Password = _configuration["RabbitMQPassword"]
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            //_channel.ExchangeDeclare(exchange: _exchange, type: ExchangeType.Fanout);
            //_queueName = _channel.QueueDeclare().QueueName;
            //_channel.QueueBind(queue: _queueName, exchange: _exchange, routingKey: "");
            _channel.ExchangeDeclare(exchange: _exchange, type: ExchangeType.Topic);
            _channel.QueueDeclare(queue: _queue, durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind(queue: _queue, exchange: _exchange, routingKey: _routingKey);

            Console.WriteLine($"--> Listenting on the Message Bus... _queueName = {_queueName}");

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
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

            //_channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
            _channel.BasicConsume(queue: _queue, autoAck: true, consumer: consumer);

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
