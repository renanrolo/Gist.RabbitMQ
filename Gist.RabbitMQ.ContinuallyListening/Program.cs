using Gist.RabbitMQ.Core;
using Gist.RabbitMQ.Core.Entities;
using Gist.RabbitMQ.Core.Queues;
using Gist.RabbitMQ.Core.Tools;
using System;
using System.Threading.Tasks;

namespace Gist.RabbitMQ.ContinuallyListening
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.WriteLine("Start");

            var queue = new PersonQueue(new QueueUow());

            queue.KeepListening((person) =>
            {
                Log.WriteLine(person.SeyHello());
                Task.Delay(300).Wait();
            });

            Log.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
