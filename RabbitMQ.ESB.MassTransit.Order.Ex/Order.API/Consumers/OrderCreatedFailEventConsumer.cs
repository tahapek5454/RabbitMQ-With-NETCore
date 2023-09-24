using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Context;
using Shared.Events;

namespace Order.API.Consumers
{
    public class OrderCreatedFailEventConsumer : IConsumer<Fault<OrderCreatedEvent>>
    {
        private readonly MassTransitDbContext _dbContext;

        public OrderCreatedFailEventConsumer(MassTransitDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<Fault<OrderCreatedEvent>> context)
        {
            Console.WriteLine("Sipariş başarısız.");

            var exceptions =context.Message.Exceptions;
            foreach (var exception in exceptions)
            {
                Console.WriteLine(exception.Message);
            }

            var order = await _dbContext.Set<Models.Order>().FirstAsync(o => o.Id == context.Message.Message.OrderId);
            order.OrderStatus = Enums.OrderStatus.Deleted;

            _dbContext.SaveChanges();
            
        }
    }
}
