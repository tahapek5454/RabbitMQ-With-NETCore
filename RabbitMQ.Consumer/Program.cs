
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Publisher;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();

// baglanti islemi
factory.Uri = new(MyConfiguration.GetResponse());

// baglanti aktiflestirme ve kanal acma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

// queue olusturma
channel.QueueDeclare(queue: "example-queue", durable: true, exclusive: false); // consumerla baglanmak amaciyla exclusive false verdik
// consumerdaki kuyruk publisherla ayni olmak zorunda



channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
// bu islemle fair patch saglamis olduk

// queue'dan mesaj okuma
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: "example-queue", autoAck: false, consumer: consumer); // ne zaman kuyruktaki consumera bilgi gelir consume et
// autoaAck false bizden mesaj beklicek


// receive bir delegate
consumer.Received += (sender, e) =>
{
    // kuyruga gelen mesajın islendigi yerdir

    // e.body = kuyruktkai mesajın verisini butunsel olarak getirir
    // e.body.span ya da e.body.ToArray() verisini byte olarak getirir.

    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
    // mesajla ilgili bildiride bulunalim
    channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false); // ilgili mesaji onayladik

};

Console.ReadLine();