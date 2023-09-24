

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

                _configurator.UseCircuitBreaker(cfg =>
                {
                    cfg.TripThreshold = 15;
                    cfg.ActiveThreshold = 10;
                    cfg.ResetInterval = TimeSpan.FromMinutes(5);
                    cfg.TrackingPeriod = TimeSpan.FromMinutes(1);

                });


            });


           
        });

    })
    .Build();

await host.RunAsync();
