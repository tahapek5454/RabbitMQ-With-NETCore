using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.API.Context;
using Order.API.ViewModels;
using Shared.Events;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly MassTransitDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;
        public OrdersController(MassTransitDbContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderVM orderVM)
        {

            Models.Order order = new Models.Order()
            {
                BuyerId = orderVM.BuyerId,
                CreatedDate = DateTime.UtcNow,
                OrderStatus = Enums.OrderStatus.Suspend,
                OrderItems = orderVM.OrderItems.Select(oi => new Models.OrderItem
                {
                    Count = oi.Count,
                    Price = oi.Price,
                    ProductId = oi.ProductId,
                }).ToList(),
                TotalPrice = orderVM.OrderItems.Sum(oi => oi.Count * oi.Price),
            };

            await _context.Set<Models.Order>().AddAsync(order);
            await _context.SaveChangesAsync();

            OrderCreatedEvent orderCreatedEvent = new()
            {
                OrderId = order.Id,
                BuyerId = order.BuyerId,
                TotalPrice = order.TotalPrice,
                OrderItems = orderVM.OrderItems.Select(oi => new OrderItemMessage()
                {
                    Count = oi.Count,
                    Price = oi.Price,
                    ProductId = oi.ProductId,
                }).ToList()
            };


            await _publishEndpoint.Publish(orderCreatedEvent);

            return Ok(true);
        }
    }
}
