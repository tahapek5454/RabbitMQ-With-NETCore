using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RabbitMQSetting
{
    public static class QueueNmaes
    {
        public static readonly string Stock_OrderCreatedQueue = "stock_order_created_queue";
        public static readonly string Stock_OrderCreatedQueueError = "stock_order_created_queue_error";
        public static readonly string Payment_StockReserved = "payment_stock_reserved";
        public static readonly string Order_PaymentSuccess = "order_payment_success";
    }
}
