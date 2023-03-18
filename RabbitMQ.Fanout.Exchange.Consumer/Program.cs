
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Fanout.Exchange.Consumer;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();

// baglanti islemi
factory.Uri = new(MyConfiguration.GetResponse());

// baglanti aktiflestirme
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

// exchange olusturma
channel.ExchangeDeclare(exchange: "fanout-exchange-example", type: ExchangeType.Fanout);

Console.WriteLine("Kuyruk Adini Giriniz: ");
string _queueName = Console.ReadLine();

// kuyruk olusturma
channel.QueueDeclare(queue:_queueName, exclusive:false);

// binding islemi
// queue ile exchange i rounting key uzerinden bind etmis oluyoruz
channel.QueueBind(queue: _queueName, exchange: "fanout-exchange-example", routingKey: string.Empty);

// mesajlari okuma
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);

};
Console.ReadLine();