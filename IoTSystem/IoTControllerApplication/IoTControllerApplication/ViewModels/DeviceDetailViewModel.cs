using System;
using System.Collections.Generic;
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

        public List<Xamarin.Forms.Cell> DeviceAttributeCells
        {
            get
            {
                List<Xamarin.Forms.Cell> cells = new List<Xamarin.Forms.Cell>();
                foreach (DeviceAttribute d in Device.Attributes)
                {
                    if (d.AttributeType == typeof(bool))
                    {
                        var cell = new Xamarin.Forms.SwitchCell();
                        cell.Text = d.Label;
                        cell.On = d.Value;
                        cells.Add(cell);
                    }
                }
                return cells;
            }

        }
    }
}
