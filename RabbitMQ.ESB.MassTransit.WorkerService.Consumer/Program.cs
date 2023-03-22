

using MassTransit;
using RabbitMQ.ESB.MassTransit.WorkerService.Consumer;
using RabbitMQ.ESB.MassTransit.WorkerService.Consumer.Consumers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(configurator =>
        {

            //consumerimizi belirleyelim
            configurator.AddConsumer<ExampleMessageConsumer>();

            configurator.UsingRabbitMq((contex, _configurator) =>
            {
                _configurator.Host(MyConfiguration.GetResponse());
                 
                //receive verdik
                _configurator.ReceiveEndpoint("example-message-queue", e => 
                e.ConfigureConsumer<ExampleMessageConsumer>(contex));   

            });


           
        });

    })
    .Build();

await host.RunAsync();
