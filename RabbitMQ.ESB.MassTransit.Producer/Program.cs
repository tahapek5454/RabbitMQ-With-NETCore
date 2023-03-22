using MassTransit;
using RabbitMQ.ESB.MassTransit.Producer;
using RabbitMQ.ESB.MassTransit.Shared.Messages;

//Uri mizi alalim
string rabbitMQUri = MyConfiguration.GetResponse();

// kuyruk olusturma
string queueName = "example-queue";

//RabbitMQ uzerinden islem yapacagimizi belirtmeliyiz
IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri);
});

// mesaji gonderecegimiz endpointi tasarlayalim (rabbitMQdaki ilgili kuyruga gonder)
ISendEndpoint sendEndpoint = await bus.GetSendEndpoint(new($"{rabbitMQUri}/{queueName}"));

Console.Write("Gonderilecek Mesaji giriniz: ");
string message = Console.ReadLine();

// simdi mesaji tip guvenli sekilde yollayalim (direkt kuyruga ozgu tek bir kuyruga yolluyoruz)
await sendEndpoint.Send<IMessage>(new ExampleMessage() { Text = message});

Console.Read();