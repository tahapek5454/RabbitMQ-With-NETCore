using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.RabbitMQSetting;
using Stock.API.Consumers;
using Stock.API.Context;

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
    configure.AddConsumer<OrderCreatedEventConsumers>();


    configure.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration.GetConnectionString("RabbitMQ"));

        configurator.ReceiveEndpoint(QueueNmaes.Stock_OrderCreatedQueue, e =>
        {
            e.ConfigureConsumer<OrderCreatedEventConsumers>(context);
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
