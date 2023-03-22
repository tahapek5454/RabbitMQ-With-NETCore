using MassTransit;
using RabbitMQ.ESB.MassTransit.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.ESB.MassTransit.WorkerService.Consumer.Consumers
{
    public class ExampleMessageConsumer : IConsumer<IMessage>
    {
        // islemler ayni mantik ayni
        public Task Consume(ConsumeContext<IMessage> context)
        {
            Console.WriteLine($" Gelen Mesaj {context.Message.Text}");

            return Task.CompletedTask;
        }
    }
}
