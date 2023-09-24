using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events;
using Stock.API.Context;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumers : IConsumer<OrderCreatedEvent>
    {
        private readonly IPublishEndpoint _endpoint;

        private readonly MassTransitDbContext _context;
        public OrderCreatedEventConsumers(IPublishEndpoint endpoint, MassTransitDbContext context)
        {
            _endpoint = endpoint;
            _context = context;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            List<bool> stockResult = new();

            foreach (OrderItemMessage orderItemMessage in context.Message.OrderItems)
            {
                stockResult.Add
                    (_context.Set<Models.Stock>().Where(s => s.ProductId == orderItemMessage.ProductId && s.Quantity >= orderItemMessage.Count).Any());
                
            }

            if(stockResult.TrueForAll(x => x.Equals(true))) {

                foreach (OrderItemMessage orderItemMessage in context.Message.OrderItems)
                {
                    Models.Stock stock = await _context.Set<Models.Stock>().FirstAsync(s => s.ProductId == orderItemMessage.ProductId);
                    stock.Quantity -= orderItemMessage.Count;

                }

                _context.SaveChanges();

                StockReservedEvent stockReservedEvent = new StockReservedEvent()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    OrderItems = context.Message.OrderItems,
                    TotalPrice = context.Message.TotalPrice
                };

                await _endpoint.Publish(stockReservedEvent);

                Console.WriteLine("Stok rezerve edildi.");


            }
            else
            {
                throw new Exception("Stok tükendi.");
            }
        }
    }
}
