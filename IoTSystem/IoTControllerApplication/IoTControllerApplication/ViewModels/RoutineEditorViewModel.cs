using IoTControllerApplication.Models;
using IoTControllerApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IoTControllerApplication.ViewModels
{
    public class RoutineEditorViewModel : BaseViewModel
    {
        public Routine routine;
        public bool modifying = false;

        public RoutineEditorViewModel()
        {
            routine = new Routine();
            modifying = false;
        }

        public RoutineEditorViewModel(Routine routine)
        {
            this.routine = new Routine(routine.ID, routine.Name, routine.TargetDeviceID, routine.TargetDriverID, routine.TargetProperty, routine.TargetValue);
            foreach (RoutineCondition rc in routine.Conditions)
            {
                this.routine.Conditions.Add(new RoutineCondition(rc.RoutineConditionID, rc.DriverID, rc.DeviceID, rc.PropertyName, rc.DesiredValue, rc.Comparison));
            }
            _TargetDevice = DeviceDataStore.DataStore.GetItemsAsync().Result.ToList().Where(x => x.DriverId == routine.TargetDriverID && x.DeviceId == routine.TargetDeviceID).First();
            _TargetProperty = _TargetDevice.Properties.Where(x => x.Label == routine.TargetProperty).First();
            modifying = true;
        }

        public string Name
        {
            get
            {
                return routine.Name;
            }
            set
            {
                routine.Name = value;
            }
        }

        private IoTDevice _TargetDevice;
        public IoTDevice TargetDevice
        {
            get
            {
                return _TargetDevice;
            }
            set
            {
                _TargetDevice = value;
                routine.TargetDeviceID = _TargetDevice.DeviceId;
                routine.TargetDriverID = _TargetDevice.DriverId;
            }
        }

        private DeviceProperty _TargetProperty;
        public DeviceProperty TargetProperty
        {
            get
            {
                return _TargetProperty;
            }
            set
            {
                _TargetProperty = value;
                routine.TargetProperty = _TargetProperty?.Label;
            }
        }

        public string TargetValue
        {
            get
            {
                return routine.TargetValue;
            }
            set
            {
                routine.TargetValue = value;
            }
        }

        public List<string> DevicesList => DeviceDataStore.DataStore.GetItemsAsync().Result.Where(x => x.IsReadOnly == false).Select(x => x.Label).ToList();

        public List<string> PropertiesList
        {
            get
            {
                try
                {
                    return _TargetDevice.Properties.Select(x => x.Label).ToList();
                }
                catch (Exception)
                {
                    return new List<string>();
                }
            }
        }

    }
}
