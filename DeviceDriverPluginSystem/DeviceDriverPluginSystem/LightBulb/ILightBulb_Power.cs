using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceDriverPluginSystem.LightBulb
{
    public interface ILightBulb_Power
    {
        bool Powered { get; set; }
    }
}
