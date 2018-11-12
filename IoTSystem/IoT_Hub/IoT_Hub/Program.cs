using System;
using DeviceDriver_TP_LINK_HS100;
using DeviceDriverPluginSystem;
using DeviceDriverPluginSystem.Generics;
using Newtonsoft.Json.Linq;

namespace IoT_Hub
{
    class Program
    {
        static void Main(string[] args)
        {
            DriverLoader.LoadDrivers();
            foreach (var s in DriverLoader.Drivers)
            {
                foreach (var q in s.Devices)
                {
                    foreach (dynamic r in q.basicDevice.DeviceVariables)
                    {
                        Console.WriteLine(r.Get());
                    }
                }
            }
            Console.ReadLine();
            //Plug dev = PlugDriver.Devices[0] as Plug;
            //(dev.DeviceVariables[0] as DeviceVariable<bool>).Set(false);
            //JToken jt = JToken.Parse(PlugDriver.GetJsonDeviceInfo(dev.ApiID)["result"]["responseData"].ToString())["system"]["get_sysinfo"]["on_time"].Value<int> == 0;
            //Console.WriteLine(jt);
            //Console.ReadLine();
        }
    }
}
