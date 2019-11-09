using Gist.RabbitMQ.Core.Entities;
using System;
using System.Collections.Generic;

namespace Gist.RabbitMQ.Core.Queues
{
    public class PersonQueue
    {
        private readonly QueueParameters _queueParameters;
        protected readonly string BaseName = "Persons";
        protected readonly QueueUow QueueUow;
        private string QueueName { get { return $"{BaseName}Queue"; } }

        public PersonQueue(QueueUow queueUow)
        {
            QueueUow = queueUow;

            QueueUow.Chanel.QueueDeclare(
                   queue: QueueName,
                   durable: true,
                   exclusive: false,
                   autoDelete: false,
                   arguments: _queueParameters?.Build()
               );
        }

        public void ReadQueue(Action<Person> callback)
        {
            QueueUow.ReadQueue<Person>(QueueName, callback);
        }

        public void KeepListening(Action<Person> callback)
        {
            QueueUow.KeepListening<Person>(QueueName, callback);
        }

        public void Push(Person person)
        {
            QueueUow.Push(QueueName, person);
        }

        public void Push(ICollection<Person> persons)
        {
            QueueUow.Push(QueueName, persons);
        }
    }
}
