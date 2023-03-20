using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Header.Exchange.Consumer;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();

// baglanti islemi
factory.Uri = new(MyConfiguration.GetResponse());

// baglanti aktiflestirme
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//exchange
channel.ExchangeDeclare(exchange: "header-exchange-example", type: ExchangeType.Headers);

Console.Write("Lutfen header valuesunu giriniz: ");
string value = Console.ReadLine();

//kuyruk olusturma
string queueName = channel.QueueDeclare().QueueName;

// kuyruk binding islemi -- headler argumentlere denk geliyor
channel.QueueBind(
    queue: queueName,
    exchange: "header-exchange-example",
    routingKey: string.Empty,
    new Dictionary<string, object>()
    {

        ["no"] = value


    });


// mesajı yakalama
EventingBasicConsumer consumer = new(channel);

channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message); 
};



Console.ReadLine();