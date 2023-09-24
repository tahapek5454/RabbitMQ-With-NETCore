using Shared.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class PaymentSuccessEvent: IEvent
    {
        public int OrderId { get; set; }
    }
}
