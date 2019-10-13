using Gist.RabbitMQ.Core;
using Gist.RabbitMQ.Core.Queues;
using Gist.RabbitMQ.Core.Tools;
using System;

namespace Gist.RabbitMQ.Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.WriteLine("Start");

            var queue = new PersonQueue(new QueueUow());
            {
                for (int i = 0; i < 1000; i++)
                {
                    string message = $"I'm number {i}";
                    Log.WriteLine($"pushing: '{message}'");

                    queue.Push(new Core.Entities.Person() { Age = i, Name = "Mr Jhon" });
                }
            }

            Log.WriteLine("Stop");

            Log.WriteLine("...");
            Console.ReadKey();
        }
    }
}
