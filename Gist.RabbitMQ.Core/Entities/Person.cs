using System;
using System.Collections.Generic;
using System.Text;

namespace Gist.RabbitMQ.Core.Entities
{
    public class Person
    {
        public int Age { get; set; }
        public string Name { get; set; }

        public string SeyHello()
        {
            return $"-Hi, I'm {Name} and I'm {Age} years old!";
        }
    }
}
