using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace IoTControllerApplication.Models
{
    public class IoTDeviceGroup : ObservableCollection<IoTDevice>
    {
        public string Title { get; private set; }

        public IoTDeviceGroup(string title)
        {
            Title = title;
        }
    }
}
