using RabbitMQ.Client;
using RabbitMQ.Header.Exchange.Producer;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();

// baglanti islemi
factory.Uri = new(MyConfiguration.GetResponse());

// baglanti aktiflestirme
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "header-exchange-example", type: ExchangeType.Headers);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes(i + " Merhaba");

    Console.Write("Lutfen Headere value'sunu giriniz : ");
    string value = Console.ReadLine();

    // header olustumaliyiz
    IBasicProperties properties = channel.CreateBasicProperties();

    properties.Headers = new Dictionary<string, object>()
    {
        ["no"] = value
    };

    channel.BasicPublish(exchange: "header-exchange-example", routingKey: string.Empty, body: message, basicProperties: properties);

}


Console.Read();