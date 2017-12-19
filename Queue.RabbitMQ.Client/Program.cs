using RabbitMQ.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue.RabbitMQ.Client {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Please enter your name.");
            var name = Console.ReadLine();
            Console.WriteLine("Please enter your message.");
            var message = Console.ReadLine();
            Console.WriteLine("Your message is being queued.");
            if (PublishQueue(name, message))
                Console.WriteLine("Your message is successfully queued.");

            Console.Read();
        }

        private static bool PublishQueue(string name, string message) {
            try {
                var factory = new ConnectionFactory { HostName = "localhost" };
                using (IConnection connection = factory.CreateConnection()) {
                    using (IModel channel = connection.CreateModel()) {
                        channel.QueueDeclare(queue: "Queue.RabbitMQ", durable: false, exclusive: false, autoDelete: false, arguments: null);
                        string queueMessage = JsonConvert.SerializeObject(new { name, message });
                        var body = Encoding.UTF8.GetBytes(queueMessage);

                        channel.BasicPublish(exchange: "", routingKey: "Queue.RabbitMQ", basicProperties: null, body: body);
                    }
                }
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
