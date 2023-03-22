
//WorkerServicelerde iligil conigurasyonlar bu sekilde veriliyor ilgili islmeler IOC ye atilir
using MassTransit;
using RabbitMQ.ESB.MassTransit.WorkerService.Publisher;
using RabbitMQ.ESB.MassTransit.WorkerService.Publisher.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(configurator =>
        {
            configurator.UsingRabbitMq((contex, _configurator) =>
            {
                _configurator.Host(MyConfiguration.GetResponse());
            });
        });

        // olusturulan servisi tanimllayalim
        services.AddHostedService<PublishMessageService>(provider =>
        {
            using IServiceScope scope = provider.CreateScope();
            IPublishEndpoint publishEndpoint = scope.ServiceProvider.GetService<IPublishEndpoint>();
            return new(publishEndpoint);
        });

    })
    .Build();

await host.RunAsync();
