
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Direct.Exchange.Consumer;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();

// baglanti islemi
factory.Uri = new(MyConfiguration.GetResponse());

// baglanti aktiflestirme
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

// Ust taraf Klasik

// publisherdaki exhange ile birebir aynı isme ve type sahip bir exhange tanımlanmaktır
// exchange olusturma islemi
channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);


// exhangein routing ket ile belirledigi kuyrugu olusutmalisin



// publisher tarafından routing key de bulunan değerdeki kuyruğa gönderilen mesajları
// kendi oluşturduğumuz kuyruga yonlendirerek tuketmemiz gerekmektedir bunu için oncelikle
// bir kuyruk olusturulmalıdır

//random isimli default ayarli queue olusturur ve bize bunun namei lazım
string queueName = channel.QueueDeclare().QueueName;


// simdi ise QueBinding islemi yapacagiz

channel.QueueBind(queue: queueName, exchange: "direct-exchange-example", routingKey: "direct-queue-example");
// routing key degerine karsilik olarak gonderilen mesajlari queue ya yonlendirmis oluyoruz queuda exchange ile bind ediliyor(Direct exchange)


// tuketim islemi
EventingBasicConsumer consumer = new(channel);

channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
};

Console.ReadLine();


