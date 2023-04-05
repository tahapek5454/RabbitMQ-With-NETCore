//Uri mizi alalim
using MassTransit;
using RabbitMQ.ESB.MassTransit.RequestResponse.Producer;
using RabbitMQ.ESB.MassTransit.Shared.RequestResponseMessages;

string rabbitMQUri = MyConfiguration.GetResponse();

//RabbitMQ uzerinden islem yapacagimizi belirtmeliyiz
IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri);
});

// ayni zamanda dinleyici de olacagindan startlaniyor
await bus.StartAsync();

string queueName = "request-queue";

//istek clientını olusturoyuruz RequestMessage turunden
IRequestClient<RequestMessage> request = bus.CreateRequestClient<RequestMessage>(new Uri($"{rabbitMQUri}/{queueName}"));


int i = 1;
while (true)
{
    await Task.Delay(200);

    // bu islemle hem mesaj gonderilir hem de response beklenir
    Response<ResponseMessage> response = await request.GetResponse<ResponseMessage>(new RequestMessage()
    {
        MessageNo= i,
        Text = $"{i}. Request"
    });

    Console.WriteLine($"Response Recived {response.Message.Text}");

    i++;
}

Console.ReadLine();