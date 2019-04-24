using IoTControllerApplication.Models;
using IoTControllerApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IoTControllerApplication.ViewModels
{
    public class RoutineConditionViewModel : BaseViewModel
    {
        public RoutineCondition condition;
        public RoutineConditionViewModel(RoutineCondition condition)
        {
            this.condition = condition;

            if (condition.DriverID != "" && condition.DeviceID != "" && condition.PropertyName != "")
            {
                _Device = DeviceDataStore.DataStore.GetItemsAsync().Result.ToList().Where(x => x.DriverId == condition.DriverID && x.DeviceId == condition.DeviceID).First();
                _Property = _Device.Properties.Where(x => x.Label == condition.PropertyName).First();
                
            }
        }

        private IoTDevice _Device;
        public IoTDevice Device
        {
            get
            {
                return _Device;
            }
            set
            {
                _Device = value;
                condition.DeviceID = _Device.DeviceId;
                condition.DriverID = _Device.DriverId;
            }
        }

        private DeviceProperty _Property;
        public DeviceProperty Property
        {
            get
            {
                return _Property;
            }
            set
            {
                _Property = value;
                condition.PropertyName = _Property?.Label;
            }
        }

        public string DesiredValue
        {
            get
            {
                return condition.DesiredValue;
            }
            set
            {
                condition.DesiredValue = value;
            }
        }

        public List<string> DevicesList => DeviceDataStore.DataStore.GetItemsAsync().Result.Select(x => x.Label).ToList();

        public List<string> PropertiesList
        {
            get
            {
                try
                {
                    return _Device.Properties.Select(x => x.Label).ToList();
                }
                catch (Exception)
                {
                    return new List<string>();
                }
            }
        }
    }
}
