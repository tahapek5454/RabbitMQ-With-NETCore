using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Message.Template.Producer;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();

// baglanti islemi
factory.Uri = new(MyConfiguration.GetResponse());

// baglanti aktiflestirme
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

// Not : Kullanilmak istenilern tasarim yorum satirindan cikarilip digerleri yorum satirina alinmali

#region P2P (Point to Point) Tasarımı

//// Exchange default olarak Direct Atiyor zaten bu tasarimda Direct Kullanilir

//// Kuyruk Olusturma

//string queueName = "example-p2p-queue";
//// consumerda olustarcagiz zaten burda yapmasak da olurdu !
//channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false);

//// mesaj tanımla
//byte[] message = Encoding.UTF8.GetBytes("merhaba");
//channel.BasicPublish(exchange: string.Empty, routingKey: queueName, body: message);


#endregion

#region Publish/Subscribe (Pub/Sub) Tasarımı

//// exchange olusturma (fanout)
//string exchangeName = "example-pub-sub-exchange";
//channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);

//// mesaj olusturma
//for (int i = 0; i < 100; i++)
//{
//    await Task.Delay(200);
//    byte[] message = Encoding.UTF8.GetBytes("Merhaba " + i);
//    channel.BasicPublish(exchange: exchangeName, routingKey: string.Empty, body: message);

//}

#endregion

#region Work Queue (İş Kuyruğu Tasarımı) Tasarımı
// queue olusturma
string qeueuName = "example-work-queue";
channel.QueueDeclare(queue: qeueuName, durable: false, exclusive: false, autoDelete: false);

// mesaj olusturma (direct exchange kullanacagiz)
for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes("Merhaba " + i);
    channel.BasicPublish(exchange: string.Empty, routingKey: qeueuName, body: message);

}

#endregion

#region Request/Response Tasarımı
//// queların olusturulması
//string requestQueue = "example-request-response-queue";
//// sıkıntı yapıyordu
////channel.QueueDeclare(queue: requestQueue, durable:false, exclusive:false, autoDelete:false);


//string replyQueueName = channel.QueueDeclare().QueueName;

//// ilgili verileri tanimak adina id olusturacagiz
//string correlationId = Guid.NewGuid().ToString();

//#region Request Mesajini olusturma ve gonderme
//// olusuturdugumuz id yi property uzerinden verelim
//IBasicProperties basicProperties = channel.CreateBasicProperties();
//basicProperties.CorrelationId = correlationId;
//// gonderilecek mesajın korelasyon degeri
//basicProperties.ReplyTo = replyQueueName;
//// gonderilecek mesaj neticesinden gelecek respons'un hangi kuyruga gonderilecegini ifade etmektedir

//// mesaj olusturma
//for (int i = 0; i < 10; i++)
//{
//    byte[] message = Encoding.UTF8.GetBytes("merhaba " + i);
//    // defaul olarak exchange direct exchange olarak kullanilacktir
//    channel.BasicPublish(exchange: string.Empty, routingKey: requestQueue, basicProperties: basicProperties, body: message);

//}

//#endregion

//#region Response Kuyrugu Dinleme
//// klasik consumer olusturacagiz
//EventingBasicConsumer consumer = new(channel);
//channel.BasicConsume(queue: replyQueueName, autoAck: true, consumer:consumer);

//consumer.Received += (sender, e) =>
//{
//    if(e.BasicProperties.CorrelationId == correlationId)
//    {
//        // eger bu bizden geldiyse oku
//        Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
//    }

//};

//#endregion

#endregion

Console.ReadLine();