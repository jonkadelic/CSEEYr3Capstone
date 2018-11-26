using IoTControllerApplication.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IoTControllerApplication.ViewModels
{
    public class DevicesViewModel : BaseViewModel
    {
        public ObservableCollection<IoTDeviceGroup> Devices { get; set; }
        public Command LoadItemsCommand { get; set; }

        public DevicesViewModel()
        {
            Title = "Devices";
            Devices = new ObservableCollection<IoTDeviceGroup>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Devices.Clear();
                var devices = await DataStore.GetItemsAsync(true);
                foreach (string manuf in devices.Select(x => x.Manufacturer))
                    Devices.Add(new IoTDeviceGroup(manuf));
                foreach (IoTDevice device in devices)
                    Devices.Where(x => x.Title == device.Manufacturer).FirstOrDefault().Add(device);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
