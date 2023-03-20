﻿using Newtonsoft.Json;

namespace RabbitMQ.Header.Exchange.Consumer
{
    public static class MyConfiguration
    {
        public static string GetResponse()
        {
            var text = File.ReadAllText("C:\\Users\\ASUS Pc\\Desktop\\VSProject\\jsonconfig\\rabbitMqConnection.json");
            var dict = JsonConvert.DeserializeObject<RemoteConnection>(text);
            return dict.ConnectionString;
        }
    }

    public class RemoteConnection
    {
        public string ConnectionString { get; set; }
    }
}
