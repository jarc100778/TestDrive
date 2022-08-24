﻿using CarCatalogService.Dtos;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace CarCatalogService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _exchange;
        private readonly string _routingKey;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
//            _exchange = _configuration["RabbitMQExchange"];
            _exchange = _configuration["RabbitMQExchange2"];
            _routingKey = _configuration["RabbitMQRoutingKey"];
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"]),
                UserName = _configuration["RabbitMQUserName"],
                Password = _configuration["RabbitMQPassword"]
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
//                _channel.ExchangeDeclare(exchange: _exchange, type: ExchangeType.Fanout);
                _channel.ExchangeDeclare(exchange: _exchange, type: ExchangeType.Topic);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                Console.WriteLine("--> Connected to MessageBus");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
            }
        }

        public void PublishCar(CarPublishedDto carPublishedDto)
        {
            var message = JsonSerializer.Serialize(carPublishedDto);

            if (_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ Connection Open, sending message...");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> RabbitMQ connectionis closed, not sending");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: _exchange,
                            //routingKey: "",
                            routingKey: _routingKey,
                            basicProperties: null,
                            body: body);
            Console.WriteLine($"--> We have sent {message}");
        }

        public void Dispose()
        {
            Console.WriteLine("MessageBus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }
    }
}
