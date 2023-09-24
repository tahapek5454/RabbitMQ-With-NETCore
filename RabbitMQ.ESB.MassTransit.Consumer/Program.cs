using MassTransit;
using RabbitMQ.ESB.MassTransit.Consumer;
using RabbitMQ.ESB.MassTransit.Consumer.Consumers;


//Uri mizi alalim
string rabbitMQUri = MyConfiguration.GetResponse();

//queue tanimlama
string queueName = "example-queue";

//RabbitMQ uzerinden islem yapacagimizi belirtmeliyiz
IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri);
    // ekstradan bu bir consumer olacagindan konfigurasyonda receive bildiricez
    factory.ReceiveEndpoint(queueName, endpoint => {

        // burdaki consumera ozel sinif olusturmaliyiz ve ona veri tipini de vermeliyiz
        endpoint.Consumer<ExampleMessageConsumer>();

        endpoint.UseRetry(r => r.Immediate(5));
    
    });
});

// artik bus i run ederek dinlemeye gecebiliriz
await bus.StartAsync();

Console.ReadLine();


