using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Queue.RabbitMQ.Consumer {
    class Program {
        static void Main(string[] args) {
            ConsumeQueue();
        }

        private static void ConsumeQueue() {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection()) {
                using (IModel channel = connection.CreateModel()) {
                    channel.QueueDeclare(queue: "Queue.RabbitMQ", durable: false, exclusive: false, autoDelete: false, arguments: null);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, e) => {
                        var body = e.Body;
                        var messageJson = Encoding.UTF8.GetString(body);
                        var obj = new { name = "", message = "" };
                        var message = JsonConvert.DeserializeAnonymousType(messageJson, obj);
                        Console.WriteLine($"Sender: {obj.name}");
                        Console.WriteLine($"Message: {obj.message}");
                    };
                    channel.BasicConsume(queue: "Queue.RabbitMQ", autoAck: true, consumer: consumer);

                    Console.ReadLine();
                }
            }
        }
        
    }
}
