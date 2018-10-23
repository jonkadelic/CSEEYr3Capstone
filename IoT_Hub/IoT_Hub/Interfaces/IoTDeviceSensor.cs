using System;
using System.Collections.Generic;

namespace IoT_Hub
{
    public interface IoTDeviceSensor
    {
		Dictionary<string, dynamic> FetchData();
    }
}
