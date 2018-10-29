using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT_Hub
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                JArray j = DeviceDriver_Lifx_LBA19E27UC10EU.MainClass.GetAllDevices();
                Console.WriteLine(j.ToString());
                Console.ReadLine();
            }
        }
    }
}
