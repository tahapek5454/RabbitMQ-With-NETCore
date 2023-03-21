using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Message.Template.Consumer;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();

// baglanti islemi
factory.Uri = new(MyConfiguration.GetResponse());

// baglanti aktiflestirme
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

// Not : Kullanilmak istenilern tasarim yorum satirindan cikarilip digerleri yorum satirina alinmali

#region P2P (Point to Point) Tasarımı

//// Kuyruk Olusturma

//string queueName = "example-p2p-queue";

//channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false);

//// consume etme islemi
//EventingBasicConsumer consumer = new(channel);
//channel.BasicConsume(queue: queueName, autoAck:false, consumer: consumer);

//consumer.Received += (sender, e) =>
//{
//    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
//};


#endregion

#region Publish/Subscribe (Pub/Sub) Tasarımı

//// Queue olustuma (defaul queue olusturduk)
//string queueName = channel.QueueDeclare().QueueName;

//// exchange olusturma (fanout)
//string exchangeName = "example-pub-sub-exchange";
//channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);

//// qeueue ile ilgili exchange'i bind edelim
//channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: string.Empty);


//// Consumer olusturma
//EventingBasicConsumer consumer = new(channel);
//channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

//consumer.Received += (sender, e) =>
//{
//    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
//};


#endregion

# region Work Queue (İş Kuyruğu Tasarımı) Tasarımı
// not direct exchange default olarak kulaniliyor
// queue olusturma
string qeueuName = "example-work-queue";
channel.QueueDeclare(queue: qeueuName, durable: false, exclusive: false, autoDelete: false);

// consumer
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: qeueuName, autoAck: true, consumer: consumer);

// olceklendirme calismasi
channel.BasicQos(0, 1, false);
// artik tum consumerlar ayni is yukune ve gorev dagilimina sahip olmus oluyor

consumer.Received += (sender, e) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
};

# endregion


#region Request/Response Tasarımı
//// Dinlenecek Queue olusturma
//string requestQueue = "example-request-response-queue";
//channel.QueueDeclare(queue: requestQueue);

//// consumer olusturma
//EventingBasicConsumer consumer = new(channel);
//channel.BasicConsume(queue: requestQueue, autoAck: true, consumer: consumer);

//consumer.Received += (sender, e) =>
//{
//    string gettingmessage = Encoding.UTF8.GetString(e.Body.Span);
//    Console.WriteLine(gettingmessage);
//    // gerekli islemler yapildi simdi response donelim

//    byte[] message = Encoding.UTF8.GetBytes("Islem Tamamlandi for " + gettingmessage);

//    // bu gonderelecek mesaja da properties tanimlayalim
//    IBasicProperties basicProperties = channel.CreateBasicProperties();
//    // ayni id yi verdik ki tanisin
//    basicProperties.CorrelationId = e.BasicProperties.CorrelationId;
    

//    channel.BasicPublish(exchange: string.Empty, routingKey:e.BasicProperties.ReplyTo, basicProperties: basicProperties, body: message);

//};





#endregion

Console.ReadLine();