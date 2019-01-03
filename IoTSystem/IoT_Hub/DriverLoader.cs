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
        private static FileSystemWatcher watcher;

        static DriverLoader()
        {
            watcher = new FileSystemWatcher()
            {
                Path = AppDomain.CurrentDomain.BaseDirectory + "plugins"
            };
            watcher.Created += new FileSystemEventHandler(ReloadDrivers);
            watcher.Deleted += new FileSystemEventHandler(ReloadDrivers); 
            watcher.EnableRaisingEvents = true;
        }

        private static void ReloadDrivers(object sender, FileSystemEventArgs e)
        {
            Drivers.Clear();
            LoadDrivers();
        }

        public static void LoadDrivers()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "plugins";
            Utility.WriteTimeStamp($"Looking for driver DLLs in {path}", typeof(DriverLoader));
            List<Assembly> assemblies = new List<Assembly>();
            foreach (string dll in Directory.GetFiles(path))
            {
                if (dll.ToLower().EndsWith(".dll"))
                    assemblies.Add(Assembly.LoadFile(dll));
            }
            Utility.WriteTimeStamp($"Found {assemblies.Count} driver DLLs", typeof(DriverLoader));
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
