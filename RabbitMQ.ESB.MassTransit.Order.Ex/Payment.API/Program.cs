using MassTransit;
using Payment.API.Consumers;
using Shared.RabbitMQSetting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumer<StockReservedEventConsumer>();

    configure.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration.GetConnectionString("RabbitMQ"));

        configurator.ReceiveEndpoint(QueueNmaes.Payment_StockReserved, e =>
        {
            e.ConfigureConsumer<StockReservedEventConsumer>(context);
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
