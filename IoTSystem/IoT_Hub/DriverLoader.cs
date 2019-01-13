using DeviceDriverPluginSystem.Generics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace IoT_Hub
{
    /// <summary>
    ///     Responsible for loading plugins into the application and converting them to objects of type Driver. 
    ///     Also monitors the plugin directory for changes, allowing for plugin addition and removal at runtime.
    /// </summary>
    public static class DriverLoader
    {
        /// <summary>
        ///     List of Drivers that have been loaded into the application through DriverLoader.
        /// </summary>
        public static List<Driver> Drivers { get; private set; } = new List<Driver>();
        /// <summary>
        ///     Object that watches /plugins/ for any file system changes.
        /// </summary>
        private static FileSystemWatcher watcher;

        /// <summary>
        ///     Static constructor that initialises the file system watcher.
        /// </summary>
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

        /// <summary>
        ///     Reloads drivers when the contents of /plugins/ is changed, by clearing the Drivers list then running the LoadDrivers function.
        /// </summary>
        /// <param name="sender">
        ///     The object that sent the event. Will always be watcher.
        /// </param>
        /// <param name="e">
        ///     Object that contains information about the file changed.
        /// </param>
        private static void ReloadDrivers(object sender, FileSystemEventArgs e)
        {
            Drivers.Clear();
            LoadDrivers();
            HttpRequestListener.UpdateListenerPrefixes();
        }

        /// <summary>
        ///     Loads all the DLLs in the /plugins/ folder into the application, and stores them as objects of type Driver in Drivers.
        /// </summary>
        public static void LoadDrivers()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "plugins";
            Utility.WriteTimeStamp($"Looking for driver DLLs in {path}", typeof(DriverLoader));
            List<Assembly> assemblies = LoadAssemblies(path);
            Utility.WriteTimeStamp($"Found {assemblies.Count} driver DLLs", typeof(DriverLoader));
            Drivers = WrapAssemblyTypesIntoDrivers(assemblies);
        }

        /// <summary>
        ///     Loads any files of type DLL into the application.
        /// </summary>
        /// <param name="path">
        ///     The location to look for DLL files.
        /// </param>
        /// <returns>
        ///     A list of all DLLs loaded into the application, as objects of type Assembly.
        /// </returns>
        private static List<Assembly> LoadAssemblies(string path)
        {
            List<Assembly> assemblies = new List<Assembly>();
            foreach (string dll in Directory.GetFiles(path))
            {
                if (dll.ToLower().EndsWith(".dll"))
                    assemblies.Add(Assembly.LoadFile(dll));
            }
            return assemblies;
        }

        /// <summary>
        ///     Creates a new instance of Driver for each type that implements AbstractBasicDriver in each assembly.
        /// </summary>
        /// <param name="assemblies">
        ///     A list of objects of type Assembly that have been loaded into the program.
        /// </param>
        /// <returns>
        ///     A list of objects of type Driver.
        /// </returns>
        private static List<Driver> WrapAssemblyTypesIntoDrivers(List<Assembly> assemblies)
        {
            List<Driver> drivers = new List<Driver>();
            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetExportedTypes())
                {
                    if (type.BaseType == typeof(AbstractBasicDriver))
                        drivers.Add(new Driver(type, type.GetProperty("DeviceType").GetValue(null) as Type));
                }
            }
            return drivers;
        }
    }
}
