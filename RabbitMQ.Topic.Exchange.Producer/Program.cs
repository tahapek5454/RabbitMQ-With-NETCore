
using RabbitMQ.Client;
using RabbitMQ.Topic.Exchange.Producer;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();

// baglanti islemi
factory.Uri = new(MyConfiguration.GetResponse());

// baglanti aktiflestirme
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//exchange belirleme

channel.ExchangeDeclare(exchange: "topic-exchange-example", type: ExchangeType.Topic);

// mesaj gonderme

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);

    byte[] message = Encoding.UTF8.GetBytes("Merhaba " + i);

    // hangi topic e gonderecegimizi kullanicidan alicaz
    Console.Write("Mesajin gonderilecegi Topici Giriniz : ");
    string topic = Console.ReadLine();
    // hangi topice gore islem belirleyecegimizi routing key e bildiriyoruz
    channel.BasicPublish(
        exchange: "topic-exchange-example",
        routingKey: topic,
        body: message
        );

}

Console.ReadLine();