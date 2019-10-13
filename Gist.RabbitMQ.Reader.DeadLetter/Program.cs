using Gist.RabbitMQ.Core;
using Gist.RabbitMQ.Core.Entities;
using Gist.RabbitMQ.Core.Queues;
using Gist.RabbitMQ.Core.Tools;
using System;
using System.Threading.Tasks;

namespace Gist.RabbitMQ.Reader.DeadLetter
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.WriteLine("Start");

            QueueUow uow = new QueueUow();

            var personQueueWithDeadLetter = new PersonDeadLetterQueue(uow);

            Log.WriteLine("..:: Generating world ::..");

            for (int i = 0; i < 100; i++)
            {
                var newPerson = new Person() { Age = i, Name = NameGenerator.Generate() };
                Log.WriteLine($"..:: Sending {newPerson.Name} to World! ::..");

                personQueueWithDeadLetter.Push(newPerson);
            }

            Log.WriteLine("..:: World is full of peoples ::..");
            Log.WriteLine("...");

            var personQueue = new PersonQueue(uow);

            Log.WriteLine("..:: Game Start ::..");
            personQueueWithDeadLetter.ReadQueue((person) =>
            {

                Log.WriteLine("...");

                Log.WriteLine("-Shhh, there's someone comming...");

                Task.Delay(500).Wait();

                RollTheDice();

                Log.WriteLine(person.SeyHello());

                personQueue.Push(person);
            });

            Log.WriteLine("Stop");
        }

        private static void RollTheDice()
        {
            Random random = new Random();

            var d20 = random.Next(1, 20);

            Log.WriteLine($":: Master rolled the dice... the result was '{d20}' ::");

            switch (d20)
            {
                case 1:
                    throw new InvalidOperationException("Gosh... you're too noob to play this game.");
                case 2:
                    throw new Exception("Ow my god, there are sharks falling from the sky!!!");
                case 3:
                    throw new DivideByZeroException("just... just get out of here...");
                case 4:
                    throw new FieldAccessException("You tries so hard, and get so far... in the end, nothing realy matters.");
                case 5:
                    throw new DllNotFoundException("I tried to find you name here...");
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                    throw new Exception("Ow come on. It's just a goblin, why can't you kill it?");
                default:
                    break;
            }

        }

    }
}
