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
channel.QueueDeclare(queue: "example-queue",durable: true ,exclusive:false); // consumerla baglanmak amaciyla exclusive false verdik
// durable true yapark kuyrugu kalici hale getirdik.

// queueya mesaj gonderme
// rabbitMQ queue ya atilan mesajlari byte kabul eder.

//byte[] message = Encoding.UTF8.GetBytes("merhaba");

//channel.BasicPublish(exchange: "", routingKey:"example-queue", body: message); // exchange bos verirsen default exchane = direct exchange

// mesajların da kalicligini saglamak için BasicPublishe IBasicProerties den bir instance olusturmak gerekir
IBasicProperties properties = channel.CreateBasicProperties();
properties.Persistent= true;

for (int i = 0; i < 100; i++)
{
    //await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes("Merhaba " + i);
    channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message, basicProperties: properties);
    // basic properties de mesajin kaliciligini saglayacaktir
}


Console.ReadLine();