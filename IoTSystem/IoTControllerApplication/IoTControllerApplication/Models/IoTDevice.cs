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
        public string DriverId { get; private set; }
        public string DeviceId { get; private set; }
        public bool IsReadOnly { get; private set; }
        public List<DeviceProperty> Properties { get; private set; }
        public string Id { get; private set; }

        public IoTDevice(string label, string name, string manufacturer, string driverId, string deviceId, bool isReadOnly)
        {
            Label = label;
            Name = name;
            Manufacturer = manufacturer;
            DriverId = driverId;
            DeviceId = deviceId;
            IsReadOnly = isReadOnly;
            Id = Guid.NewGuid().ToString();
            Properties = new List<DeviceProperty>();
        }
    }
}
