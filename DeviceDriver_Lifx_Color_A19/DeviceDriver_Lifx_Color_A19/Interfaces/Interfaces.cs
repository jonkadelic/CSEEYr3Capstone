using System;
using System.Collections.Generic;
using System.Text;
using DeviceDriverPluginSystem;

namespace DeviceDriver_Lifx_Color_A19.Interfaces
{
    public interface IPowered : IDeviceValue
    {
        bool Value { get; set; }
        string Name { get; }
        bool MaxValue { get; }
        bool MinValue { get; }
    }

    public interface IHue : IDeviceValue
    {
        int Value { get; set; }
        string Name { get; }
        int MaxValue { get; }
        int MinValue { get; }
    }
    public interface ISaturation : IDeviceValue
    {
        double Value { get; set; }
        string Name { get; }
        double MaxValue { get; }
        double MinValue { get; }
    }
    public interface IBrightness : IDeviceValue
    {
        double Value { get; set; }
        string Name { get; }
        double MaxValue { get; }
        double MinValue { get; }
    }

    public interface IWarmth : IDeviceValue
    {
        int Value { get; set; }
        string Name { get; }
        int MaxValue { get; }
        int MinValue { get; }
    }
}
