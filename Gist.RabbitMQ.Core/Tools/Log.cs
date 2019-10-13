using System;

namespace Gist.RabbitMQ.Core.Tools
{
    public static class Log
    {
        public static void WriteLine(string message)
        {
            Console.WriteLine($"{DateTime.Now}: {message}");
        }

        public static void Write(string message)
        {
            Console.Write(message);
        }
    }
}
