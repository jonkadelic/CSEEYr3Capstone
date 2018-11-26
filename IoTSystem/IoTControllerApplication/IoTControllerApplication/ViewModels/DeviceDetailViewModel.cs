using System;

using IoTControllerApplication.Models;

namespace IoTControllerApplication.ViewModels
{
    public class DeviceDetailViewModel : BaseViewModel
    {
        public IoTDevice Device { get; set; }
        public DeviceDetailViewModel(IoTDevice device = null)
        {
            Title = device?.Label;
            Device = device;
        }
    }
}
