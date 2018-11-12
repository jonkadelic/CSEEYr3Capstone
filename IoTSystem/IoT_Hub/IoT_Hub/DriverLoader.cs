using DeviceDriverPluginSystem.Generics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace IoT_Hub
{
    public class DriverLoader
    {
        public static List<Driver> Drivers = new List<Driver>();

        public static void LoadDrivers()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "plugins/";
            List<Assembly> assemblies = new List<Assembly>();
            foreach (string dll in Directory.GetFiles(path, "*.DLL"))
            {
                assemblies.Add(Assembly.LoadFile(dll));
            }
            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetExportedTypes())
                {
                    if (type.BaseType == typeof(AbstractBasicDriver))
                    {
                        Drivers.Add(new Driver(type, type.InvokeMember("get_DeviceType", BindingFlags.InvokeMethod, null, null, null) as Type));
                    }
                }
            }
        }
    }
}
