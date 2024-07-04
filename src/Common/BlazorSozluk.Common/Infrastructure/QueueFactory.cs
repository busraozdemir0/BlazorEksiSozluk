﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorSozluk.Common.Infrastructure
{
    public static class QueueFactory
    {
        // https://gokhan-gokalp.com/rabbitmq-nedir-ve-windowsa-kurulumu/

        // RabbitMQ'ye mesaj gonderebilmek icin
        public static void SendMessageToExchange(string exchangeName, string exchangeType, string queueName, object obj)
        {
            var channel = CreateBasicConsumer()
                .EnsureExchange(exchangeName, exchangeType) // Exchange'nin yaratildigindan emin olmak icin
                .EnsureQueue(queueName, exchangeName) // Queue'nin yaratildigindan emin olmak icin
                .Model;

            var body=Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj)); // GetBytes metodu string deger istedigi icin obj'yi Serialize ile string'e cevirdik

            channel.BasicPublish(exchange: exchangeName,
                                routingKey: queueName, 
                                basicProperties: null,
                                body: body);
        }

        public static EventingBasicConsumer CreateBasicConsumer()
        {
            var factory = new ConnectionFactory() { HostName = SozlukConstants.RabbitMQHost };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            return new EventingBasicConsumer(channel);
        }

        // Exchange'nin yaratildigindan emin olacagiz
        public static EventingBasicConsumer EnsureExchange(this EventingBasicConsumer consumer, string exchangeName, string exchangeType = SozlukConstants.DefaultExchangeType)
        {
            consumer.Model.ExchangeDeclare(exchange: exchangeName,
                                            type: exchangeType,
                                            durable: false,
                                            autoDelete: false);
            return consumer;
        }

        // Queue'nin yaratildigindan emin olacagiz
        public static EventingBasicConsumer EnsureQueue(this EventingBasicConsumer consumer, string queueName, string exchangeName)
        {
            consumer.Model.QueueDeclare(queue: queueName,
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        null);

            consumer.Model.QueueBind(queueName, exchangeName, queueName);
            return consumer;
        }
    }

}
