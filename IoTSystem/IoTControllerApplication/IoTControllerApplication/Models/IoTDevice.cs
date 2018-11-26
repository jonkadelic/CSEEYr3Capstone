using System;
using System.Collections.Generic;
using System.Text;

namespace IoTControllerApplication.Models
{
    public class IoTDevice
    {  
        public string Label { get; private set; }
        public string Name { get; private set; }
        public string Manufacturer { get; private set; }
        public int DriverId { get; private set; }
        public int DeviceId { get; private set; }
        public List<DeviceAttribute> Attributes { get; private set; }
        public string Id { get; private set; }

        public IoTDevice(string label, string name, string manufacturer, int driverId, int deviceId)
        {
            Label = label;
            Name = name;
            Manufacturer = manufacturer;
            DriverId = driverId;
            DeviceId = deviceId;
            Id = Guid.NewGuid().ToString();
            Attributes = new List<DeviceAttribute>();
        }
    }
}
