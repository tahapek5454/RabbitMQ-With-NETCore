
using RabbitMQ.Client;
using RabbitMQ.Direct.Exchange.Producer;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();

// baglanti islemi
factory.Uri= new (MyConfiguration.GetResponse());

// baglanti aktiflestirme
using IConnection connection= factory.CreateConnection();
using IModel channel = connection.CreateModel();

// Ust taraf Klasik

// exchange olusturma islemi
channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

// exhange olusturdugunda ayriyetten bir queue olusturmuyorsun, queue yu consumer olusturuyor

while (true)
{
    Console.Write("Mesaj Giriniz: ");
    string? message = Console.ReadLine();
    byte[] byteMessage = Encoding.UTF8.GetBytes(message);

    // yayınlama islemi
    channel.BasicPublish(
        exchange: "direct-exchange-example",
        routingKey: "direct-queue-example",
        body: byteMessage
    
        );
}

