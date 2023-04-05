using MassTransit;
using RabbitMQ.ESB.MassTransit.Shared.RequestResponseMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.ESB.MassTransit.RequestResponse.Consumer.Consumer
{
    public class RequestMessageConsumer : IConsumer<RequestMessage>
    {
        public async Task Consume(ConsumeContext<RequestMessage> context)
        {
            //gerekli islemler yapilacak
            Console.WriteLine(context.Message.Text);

            //response gonderme
            await context.RespondAsync<ResponseMessage>(new ResponseMessage()
            {
                Text = $"{context.Message.MessageNo}. response to request"
            });
        }
    }
}
