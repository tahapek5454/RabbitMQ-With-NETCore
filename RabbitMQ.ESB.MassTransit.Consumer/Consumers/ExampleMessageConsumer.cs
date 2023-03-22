using MassTransit;
using RabbitMQ.ESB.MassTransit.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.ESB.MassTransit.Consumer.Consumers
{
    public class ExampleMessageConsumer : IConsumer<IMessage>
    {
        //ilgili kuyruga IMessage turunden bir mesaj gelirse bu consumer
        // consume fonksiyonunu tetikleyecek ve contex uzerinden mesaji elde edecek
        // ve islememizi saglayacaktir
        public Task Consume(ConsumeContext<IMessage> context)
        {
            Console.WriteLine("Gonderilen Mesaj : " + context.Message.Text);

            return Task.CompletedTask;
        }
    }
}
