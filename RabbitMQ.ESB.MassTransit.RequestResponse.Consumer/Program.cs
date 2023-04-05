//Uri mizi alalim
using MassTransit;
using RabbitMQ.ESB.MassTransit.RequestResponse.Consumer;
using RabbitMQ.ESB.MassTransit.RequestResponse.Consumer.Consumer;

string rabbitMQUri = MyConfiguration.GetResponse();

string queueName = "request-queue";

//RabbitMQ uzerinden islem yapacagimizi belirtmeliyiz
IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri);

    //consumer configurasyonunu yapalim
    factory.ReceiveEndpoint(queueName, endpoint =>
    {
        endpoint.Consumer<RequestMessageConsumer>();
    });
});

//bus nesnesini start ederek dinlemeyi baslaatilim

await bus.StartAsync();

Console.ReadLine();