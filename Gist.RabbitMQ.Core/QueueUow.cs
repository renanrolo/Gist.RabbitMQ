using Gist.RabbitMQ.Core.Tools;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections;
using System.Text;

namespace Gist.RabbitMQ.Core
{
    public class QueueUow : IDisposable
    {
        private IConnection _connection;
        private IModel _model;
        private IBasicProperties _basicProperties { get; set; }
        private IBasicProperties BasicProperties
        {
            get
            {
                if (_basicProperties == null)
                    _basicProperties = RabbitMQExtended.CreateBasicProperties(_model);

                return _basicProperties;
            }
        }

        protected readonly ConnectionFactory _connectionFactory;
        public IModel Chanel
        {
            get
            {
                if (_connection is null || !_connection.IsOpen)
                {
                    Log.WriteLine("Opening RabbitMQ connection.");
                    _connection = _connectionFactory.CreateConnection();
                }

                if (_model is null || !_model.IsOpen)
                {
                    Log.WriteLine("Opening RabbitMQ chanel.");
                    _model = _connection.CreateModel();
                }

                return _model;
            }
        }

        public QueueUow()
        {
            _connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
        }

        public void Dispose()
        {
            CloseConnection();
        }

        public void CloseConnection()
        {
            if (!_model.IsClosed)
            {
                Console.WriteLine("Closing RabbitMQ chanel.");

                _model.Close();
            }

            if (_connection.IsOpen)
            {
                Console.WriteLine("Closing RabbitMQ connection.");
                _connection.Close();
            }
        }

        public void ReadQueue<T>(string queueName, Action<T> callback)
        {
            while (Chanel.BasicGet(queueName, false) is BasicGetResult response && response != null)
            {
                T message = RabbitMQExtended.DeserializeResponse<T>(response.Body);

                callback(message);

                Chanel.BasicAck(response.DeliveryTag, false);
            }
        }

        public void Push(string queueName, object data)
        {
            Chanel.ConfirmSelect();

            if (data is ICollection collection)
            {
                foreach (var item in collection)
                {
                    var jsonData = JsonConvert.SerializeObject(item);
                    Chanel.BasicPublish("", queueName, BasicProperties, Encoding.UTF8.GetBytes(jsonData));
                }
            }
            else
            {
                var jsonData = JsonConvert.SerializeObject(data);
                Chanel.BasicPublish("", queueName, BasicProperties, Encoding.UTF8.GetBytes(jsonData));
            }

            Chanel.WaitForConfirmsOrDie();
            Chanel.WaitForConfirms();
        }

        public void KeepListening<T>(string queueName, Action<T> callback)
        {
            var consumer = new EventingBasicConsumer(Chanel);

            //Get one message at time. That's good for more than one consumer per queue.
            Chanel.BasicQos(0, 1, false);

            consumer.Received += (model, ea) =>
            {
                T message = RabbitMQExtended.DeserializeResponse<T>(ea.Body);
                callback(message);
                Chanel.BasicAck(ea.DeliveryTag, false);
            };

            Chanel.BasicConsume(queue: queueName,
                                autoAck: false,
                                consumer: consumer);
        }

    }
}
