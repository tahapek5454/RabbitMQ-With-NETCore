using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Context;
using Shared.Events;

namespace Order.API.Consumers
{
    public class PaymentSuccessEventConsumer : IConsumer<PaymentSuccessEvent>
    {
        private readonly MassTransitDbContext _context;

        public PaymentSuccessEventConsumer(MassTransitDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<PaymentSuccessEvent> context)
        {
            var order = await _context.Set<Models.Order>().FirstAsync(o => o.Id == context.Message.OrderId);

            order.OrderStatus = Enums.OrderStatus.Create;

            _context.SaveChanges();
        }
    }
}
