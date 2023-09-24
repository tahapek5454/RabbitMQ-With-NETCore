using MassTransit;
using Shared.Events;

namespace Payment.API.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        private readonly IPublishEndpoint _endpoint;

        public StockReservedEventConsumer(IPublishEndpoint endpoint)
        {
            _endpoint = endpoint;
        }

        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            Console.WriteLine("İşlemler tamamlandı.");

            Console.WriteLine($"{context.Message.OrderId} numaralı sipariş için ödenecek tutar {context.Message.TotalPrice} TL dir");

            PaymentSuccessEvent paymentSuccessEvent = new PaymentSuccessEvent()
            {
                OrderId = context.Message.OrderId,
            };

            await _endpoint.Publish(paymentSuccessEvent);   
        }
    }
}
