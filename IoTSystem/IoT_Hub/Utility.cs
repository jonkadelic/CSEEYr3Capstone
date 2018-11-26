using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT_Hub
{
    public static class Utility
    {
        public static void WriteTimeStamp(string text)
        {
            Console.WriteLine($"~{DateTime.Now}~ {text}");
        }
        public static void WriteTimeStamp(string text, Type t)
        {
            WriteTimeStamp($"{t.Name}: {text}");
        }
    }
}
