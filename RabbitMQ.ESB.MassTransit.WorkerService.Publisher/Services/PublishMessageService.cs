﻿using MassTransit;
using RabbitMQ.ESB.MassTransit.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.ESB.MassTransit.WorkerService.Publisher.Services
{
    public class PublishMessageService : BackgroundService
    {
        readonly IPublishEndpoint _publishEndpoint;

        public PublishMessageService(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // mesaj gonderme islemi
            int i = 0;
            while (true)
            {
                ExampleMessage message = new()
                {
                    Text = $" {++i}  mesaj"
                };
                // publishin genel davranisi tum kuyruklara mesaj gondemresidir
                await _publishEndpoint.Publish(message);

            }
        }
    }
}
