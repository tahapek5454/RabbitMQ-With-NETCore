using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Consumers;
using Order.API.Context;
using Shared.RabbitMQSetting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MassTransitDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL"));
});



builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumer<OrderCreatedFailEventConsumer>();
    configure.AddConsumer<PaymentSuccessEventConsumer>();

    configure.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration.GetConnectionString("RabbitMQ"));

        configurator.ReceiveEndpoint(QueueNmaes.Order_PaymentSuccess, e =>
        {
            e.ConfigureConsumer<PaymentSuccessEventConsumer>(context);
            e.DiscardSkippedMessages();
        });

        configurator.ReceiveEndpoint(QueueNmaes.Stock_OrderCreatedQueueError, e =>
        {
            e.ConfigureConsumer<OrderCreatedFailEventConsumer>(context);
            e.DiscardSkippedMessages();
        });
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
