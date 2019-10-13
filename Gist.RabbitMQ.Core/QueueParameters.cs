using System;
using System.Collections.Generic;

namespace Gist.RabbitMQ.Core
{
    public class QueueParameters
    {
        public bool LazyQueueMode { get; private set; }
        public int? TimeToLive { get; private set; }
        public string DeadLetterExchange { get; private set; }

        public QueueParameters(bool lazyQueueMode = true, int? timeToLive = null, string deadLetterExchange = null)
        {
            LazyQueueMode = lazyQueueMode;
            TimeToLive = timeToLive;
            DeadLetterExchange = deadLetterExchange;
        }

        public Dictionary<string, object> Build()
        {
            var queueParams = new Dictionary<string, object>();

            if (LazyQueueMode)
            {
                queueParams.Add("x-queue-mode", "lazy");
            }

            if (TimeToLive.HasValue)
            {
                queueParams.Add("x-message-ttl", TimeToLive.Value);
            }

            if (!String.IsNullOrEmpty(DeadLetterExchange))
            {
                queueParams.Add("x-dead-letter-exchange", DeadLetterExchange);
            }

            return queueParams;
        }
    }
}
