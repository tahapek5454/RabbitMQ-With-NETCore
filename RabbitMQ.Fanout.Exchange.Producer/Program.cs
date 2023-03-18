
using RabbitMQ.Client;
using RabbitMQ.Fanout.Exchange.Producer;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();

// baglanti islemi
factory.Uri = new(MyConfiguration.GetResponse());

// baglanti aktiflestirme
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

// exchange olusturma
channel.ExchangeDeclare(exchange: "fanout-exchange-example", type: ExchangeType.Fanout);


// mesaj yayımlama

for(int i = 0;i < 100; i++)
{
    await Task.Delay(200);

    byte[] message = Encoding.UTF8.GetBytes("Merhaba");

    //fanout exhange de herhangi bir kuyrugu ayirt etmeyecegimizden dolayi routing key e bos deger veriyoruz
    channel.BasicPublish(exchange: "fanout-exchange-example", routingKey: string.Empty, body:message);
}

Console.ReadLine();