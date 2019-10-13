using Gist.RabbitMQ.Core;
using Gist.RabbitMQ.Core.Queues;
using Gist.RabbitMQ.Core.Tools;
using System.Threading.Tasks;

namespace Gist.RabbitMQ.Reader
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.WriteLine("Start");

            var queue = new PersonQueue(new QueueUow());

            queue.ReadQueue((person) =>
            {
                Log.WriteLine(person.SeyHello());
                Task.Delay(300).Wait();
            });

            Log.WriteLine("Stop");
        }
    }
}
