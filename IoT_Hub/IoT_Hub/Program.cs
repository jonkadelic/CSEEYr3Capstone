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
            DeviceDriver_Lifx_Color_A19.Device.GetDevices();
            var d = DeviceDriver_Lifx_Color_A19.Device.Devices.First();
            d.Powered = false;
            Console.ReadLine();
            d.Powered = true;
            Console.ReadLine();
            d.Hue = 0;
            Console.ReadLine();
            d.Hue = 180;
            Console.ReadLine();
            d.Saturation = 0.0d;
            Console.ReadLine();
            d.Saturation = 1.0d;
            Console.ReadLine();
            d.Lightness = 0.1d;
            Console.ReadLine();
            d.Lightness = 1.0d;
            Console.ReadLine();
            d.Warmth = 1500;
            Console.ReadLine();
            d.Warmth = 9000;
            Console.ReadLine();
        }
    }
}
