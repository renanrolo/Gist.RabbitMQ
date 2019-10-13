using Gist.RabbitMQ.Core.Entities;
using Gist.RabbitMQ.Core.Tools;
using System;

namespace Gist.RabbitMQ.Core.Queues
{
    public class PersonDeadLetterQueue
    {
        private readonly QueueParameters _queueParameters;
        protected readonly string BaseName = "PersonsDeadLetter";
        protected readonly QueueUow QueueUow;
        private string ExcangeName { get { return $"{BaseName}Exchange"; } }
        private string QueueName { get { return $"{BaseName}1Queue"; } }
        private string RetryQueue { get { return $"{BaseName}2QueueRetry"; } }
        private string ErrorQueue { get { return $"{BaseName}3QueueError"; } }

        public PersonDeadLetterQueue(QueueUow queueUow)
        {
            QueueUow = queueUow;

            //normal queue
            QueueUow.Chanel.QueueDeclare(
                   queue: QueueName,
                   durable: true,
                   exclusive: false,
                   autoDelete: false,
                   arguments: null
               );

            var deadLetterParameters = new QueueParameters(timeToLive: 15000, deadLetterExchange: ExcangeName);

            //retry queue
            QueueUow.Chanel.QueueDeclare(
                   queue: RetryQueue,
                   durable: true,
                   exclusive: false,
                   autoDelete: false,
                   arguments: deadLetterParameters.Build()
               );

            //error queue
            QueueUow.Chanel.QueueDeclare(
                   queue: ErrorQueue,
                   durable: true,
                   exclusive: false,
                   autoDelete: false,
                   arguments: null
               );

            QueueUow.Chanel.ExchangeDeclare(
                            exchange: ExcangeName,
                            type: "fanout",
                            durable: true,
                            autoDelete: false,
                            arguments: null
                        );

            QueueUow.Chanel.QueueBind(
                    queue: QueueName,
                    exchange: ExcangeName,
                    routingKey: string.Empty,
                    arguments: null
                );
        }

        public void ReadQueue(Action<Person> callback)
        {
            QueueUow.ReadQueue<Envelop<Person>>(
               QueueName,
                (envelop) =>
                {
                    try
                    {
                        callback(envelop.Content);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteLine(ex.Message);

                        envelop.Exceptions.Add(ex);
                        ManageError(envelop);
                    }
                });
        }

        private void ManageError(Envelop<Person> envelop)
        {
            if (envelop.RetriesCount <= 2)
            {
                envelop.RetriesCount++;
                Log.WriteLine($":: Inserting {envelop.Content.Name} to retry queue ::");
                QueueUow.Push(RetryQueue, envelop);
            }
            else
            {
                Log.WriteLine($":: Inserting {envelop.Content.Name} to error queue ::");
                QueueUow.Push(ErrorQueue, envelop);
            }
        }

        public void Push(Person person)
        {
            Envelop<Person> envelop = new Envelop<Person>(person);
            QueueUow.Push(QueueName, envelop);
        }
    }
}
