# Gist.RabbitMQ
Simple project to see the gist of RabbitMQ package

* How to use RabbitMq on .net core
* Segregate od Duties:
    * Unity of Work class is responsable for connectivity.
    * Queues classes are responsible for create its on queue, consume and publish messages.

#### libs
```
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
```

#### Setup
```
var connectionFactory = new ConnectionFactory()
{
    HostName = "localhost",
    Port = 5672,
    UserName = "guest",
    Password = "guest"
};

IConnection connection = connectionFactory.CreateConnection();

IModel model = connection.CreateModel();

IBasicProperties basicProperties = model.CreateBasicProperties();
basicProperties.ContentType = "text/plain";
basicProperties.DeliveryMode = 2;
basicProperties.Persistent = true;
```

#### Sending
```
model.ConfirmSelect();

var jsonData = JsonConvert.SerializeObject("Hello World!");
model.BasicPublish("", "QueueName", basicProperties, Encoding.UTF8.GetBytes(jsonData));

model.WaitForConfirmsOrDie();
model.WaitForConfirms();
```

#### Recieving
```
bool keepReading = true;
do
{
    BasicGetResult response = model.BasicGet("QueueName", false);

    if (response != null)
    {
        var responseBody = Encoding.UTF8.GetString(response.Body);

        string message = JsonConvert.DeserializeObject<string>(responseBody);

        Console.WriteLine(message);

        model.BasicAck(response.DeliveryTag, false);
    }
    else
    {
        keepReading = false;
    }

} while (keepReading);
```
