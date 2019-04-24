using DeviceDriverPluginSystem.Generics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IoT_Hub
{
    /// <summary>
    ///     Represents a driver added through the plugin architecture. Stores the Types that inherit AbstractBasicDriver and AbstractBasicDevice.
    /// </summary>
    public class Driver
    {
        /// <summary>
        ///     The unique ID for an instance of this class.
        /// </summary>
        public string Id { get; }
        /// <summary>
        ///     Fetches the name of the device from the Type name.
        /// </summary>
        public string Name { get; }
        /// <summary>
        ///     A Type that implements AbstractBasicDriver.
        /// </summary>
        private readonly Type driverType;
        /// <summary>
        ///     A Type that implements AbstractBasicDevice.
        /// </summary>
        private readonly Type deviceType;

        /// <summary>
        ///     Constructor for the driver. Stores the driver Type and device Type, as well as assigning a unique driver ID.
        /// </summary>
        /// <param name="driverType">
        ///     A Type that implements AbstractBasicDriver.
        /// </param>
        /// <param name="deviceType">
        ///     A Type that implements AbstractBasicDevice.
        /// </param>
        public Driver(Type driverType, Type deviceType)
        {
            this.driverType = driverType;
            this.deviceType = deviceType;
            Id = driverType.Assembly.GetName().Name;
            Name = driverType.Name;
        }

        /// <summary>
        ///     Fetches the list of Devices that belong to an instance of Driver.
        /// </summary>

        List<DriverDevice> driverDeviceCache;
        DateTime lastRequest = DateTime.Now;

        public List<DriverDevice> Devices
        {
            get
            {
                Utility.WriteTimeStamp($"Request for devices of driver {Name}", typeof(Driver));
                if (driverDeviceCache == null || lastRequest.Add(new TimeSpan(0, 5, 0)) < DateTime.Now)
                {
                    try
                    {
                        List<AbstractBasicDevice> allGenericDevices = driverType.GetProperty("Devices").GetValue(null) as List<AbstractBasicDevice>;
                        driverDeviceCache = allGenericDevices.Select(x => new DriverDevice(deviceType, x)).ToList() ?? new List<DriverDevice>();
                    }
                    catch (Exception)
                    {
                        driverDeviceCache = null;
                    }
                    lastRequest = DateTime.Now;
                }
                return driverDeviceCache;

            }
        }
    }
}
