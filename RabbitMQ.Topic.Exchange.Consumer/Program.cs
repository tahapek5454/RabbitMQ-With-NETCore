
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Topic.Exchange.Consumer;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();

// baglanti islemi
factory.Uri = new(MyConfiguration.GetResponse());

// baglanti aktiflestirme
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//exchange belirleme

channel.ExchangeDeclare(exchange: "topic-exchange-example", type: ExchangeType.Topic);

Console.Write("Dinlenecek Topic i giriniz ");
string topic = Console.ReadLine();

// queue olusturma o kendisi name atsin biz o nami alalim
string queueName = channel.QueueDeclare().QueueName;

//binding

channel.QueueBind(queue: queueName, exchange: "topic-exchange-example", routingKey: topic);

//MESAJ
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);

};

Console.ReadLine();