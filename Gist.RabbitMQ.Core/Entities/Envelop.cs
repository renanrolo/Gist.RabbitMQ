using System;
using System.Collections.Generic;

namespace Gist.RabbitMQ.Core.Entities
{
    public class Envelop<T>
    {
        public int RetriesCount { get; set; }
        public T Content { get; set; }
        public List<Exception> Exceptions { get; set; }

        public Envelop()
        {
            Exceptions = new List<Exception>();
        }

        public Envelop(T content)
        {
            Exceptions = new List<Exception>();
            Content = content;
        }
    }
}
