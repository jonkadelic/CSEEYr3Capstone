using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceDriverPluginSystem;
using DeviceDriverPluginSystem.Generics;

namespace DeviceDriver_Environmental
{
    public class Environmental : AbstractBasicDevice
    {
        public new string Label => "Environmental";

        public new string Name => "Environmental";

        public new string Manufacturer => "Internal";

        public new string Id => "9999999";

        internal Environmental()
        {
            PopulateDeviceVariables();
        }

        private void PopulateDeviceVariables()
        {
            AddDeviceVariable("Time", GetTime);
            AddDeviceVariable("Temperature", GetTemperature);
            AddDeviceVariable("Humidity", GetHumidity);
            AddDeviceVariable("Pressure", GetPressure);
            AddDeviceVariable("WindSpeed", GetWindSpeed);
            AddDeviceVariable("WindDirection", GetWindDirection);
            AddDeviceVariable("Clouds", GetClouds);
            AddDeviceVariable("Visibility", GetVisibility);
            AddDeviceVariable("Precipitation", GetPrecipitation);
        }

        private DateTime GetTime()
        {
            return DateTime.Now;
        }

        public void AddDeviceVariable<VariableType>(string label, Func<VariableType> getter) where VariableType : IComparable
        {
            DeviceAttributes.Add(new DeviceAttribute<VariableType>(getter, NullSet, label));
        }

        private void NullSet<VariableType>(VariableType t)
        {
            return;
        }

        private double GetTemperature()
        {
            EnvironmentalDriver.ReloadData();
            return EnvironmentalDriver.temperature;
        }

        private int GetHumidity()
        {
            EnvironmentalDriver.ReloadData();
            return EnvironmentalDriver.humidity;
        }

        private double GetPressure()
        {
            EnvironmentalDriver.ReloadData();
            return EnvironmentalDriver.pressure;
        }

        private double GetWindSpeed()
        {
            EnvironmentalDriver.ReloadData();
            return EnvironmentalDriver.windSpeed;
        }

        private double GetWindDirection()
        {
            EnvironmentalDriver.ReloadData();
            return EnvironmentalDriver.windDirection;
        }

        private int GetClouds()
        {
            EnvironmentalDriver.ReloadData();
            return EnvironmentalDriver.clouds;
        }

        private int GetVisibility()
        {
            EnvironmentalDriver.ReloadData();
            return EnvironmentalDriver.visibility;
        }

        private int GetPrecipitation()
        {
            EnvironmentalDriver.ReloadData();
            return EnvironmentalDriver.precipitation;
        }
    }
}
