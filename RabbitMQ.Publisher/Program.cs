using RabbitMQ.Client;
using RabbitMQ.Publisher;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();

// baglanti islemi
factory.Uri = new(MyConfiguration.GetResponse());

// baglanti aktiflestirme ve kanal acma
using IConnection connection= factory.CreateConnection();
using IModel channel = connection.CreateModel();

// queue olusturma
channel.QueueDeclare(queue: "example-queue", exclusive:false); // consumerla baglanmak amaciyla exclusive false verdik

// queueya mesaj gonderme
// rabbitMQ queue ya atilan mesajlari byte kabul eder.

byte[] message = Encoding.UTF8.GetBytes("merhaba");

channel.BasicPublish(exchange: "", routingKey:"example-queue", body: message); // exchange bos verirsen default exchane = direct exchange

Console.ReadLine();