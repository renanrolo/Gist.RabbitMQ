using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Gist.RabbitMQ.Core
{
    internal class RabbitMQExtended
    {
        internal static T DeserializeResponse<T>(byte[] response)
        {
            var responseBody = Encoding.UTF8.GetString(response);
            return JsonConvert.DeserializeObject<T>(responseBody);
        }


        internal static IBasicProperties CreateBasicProperties(IModel model)
        {
            IBasicProperties basicProperties = model.CreateBasicProperties();
            basicProperties.ContentType = "text/plain";
            basicProperties.DeliveryMode = 2;
            basicProperties.Persistent = true;

            return basicProperties;
        }
    }
}
