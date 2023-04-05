using Newtonsoft.Json;

namespace RabbitMQ.ESB.MassTransit.RequestResponse.Producer
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
