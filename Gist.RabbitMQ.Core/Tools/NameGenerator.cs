using System;
using System.Collections.Generic;
using System.Text;

namespace Gist.RabbitMQ.Core.Tools
{
    public static class NameGenerator
    {
        public static string Generate()
        {
            Random random = new Random();

            var d20 = random.Next(1, 20);

            return Names[d20 - 1];
        }

        private static readonly string[] Names =  { "Aniyah Hubbard",
                 "Terry Jarvis",
                "Marjorie Stubbs",
                "Crystal Dalby",
                "Hadi Mcnally",
                "Aleyna Noble",
                "Osian Lutz",
                "Jevan Kouma",
                "Tegan Pritchard",
                "Ayush Rosales",
                "Dianne Hendrix",
                "Iain Bellamy",
                "Jessie Flynn",
                "Gracie -Mai Larson",
                "Jarred Guthrie",
                "Harriette Crouch",
                "Romany Pope",
                "Kamila Cartwright",
                "Selena Braun",
                "Lowri Bennett"
        };
    }
}
