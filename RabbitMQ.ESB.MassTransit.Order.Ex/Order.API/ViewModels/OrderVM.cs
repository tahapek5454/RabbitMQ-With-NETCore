using Order.API.Models;

namespace Order.API.ViewModels
{
    public class OrderVM
    {
        public int BuyerId { get; set; }
        public List<OrderItemVm> OrderItems { get; set; }
    }


    public class OrderItemVm
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
    }
}
