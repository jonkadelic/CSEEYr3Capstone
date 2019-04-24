using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using IoTControllerApplication.Models;
using Xamarin.Forms;

namespace IoTControllerApplication.ViewModels
{
    public class DeviceDetailViewModel : BaseViewModel
    {
        public Command ReloadCommand { get; set; }

        public ObservableCollection<Cell> CellList { get; set; }


        public IoTDevice Device { get; set; }
        public DeviceDetailViewModel(IoTDevice device = null)
        {
            Title = device?.Label;
            Device = device;
            ReloadCommand = new Command(async () => await ExecuteReload());
            CellList = new ObservableCollection<Cell>();
        }


        public async Task ExecuteReload()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                CellList.Clear();
                var cells = await GetDevicePropertyCellsAsync();
                foreach (Cell c in cells)
                    CellList.Add(c);
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

        public async Task<List<Cell>> GetDevicePropertyCellsAsync()
        {
            Dictionary<int, Task<Cell>> cellTaskList = new Dictionary<int, Task<Cell>>();
            for (int i = 0; i < Device.Properties.Count; i++)
            {
                cellTaskList.Add(i, GetValueCellAsync(Device.Properties[i]));
            }
            Cell[] cells = new Cell[Device.Properties.Count];
            foreach (var c in cellTaskList)
            {
                Task<Cell> finishedTask = await Task.WhenAny(c.Value).ConfigureAwait(false);
                cells[c.Key] = await finishedTask.ConfigureAwait(false);
            }
            return cells.ToList();
        }

        private async Task<Cell> GetValueCellAsync(DeviceProperty d)
        {
            dynamic val = await d.GetValueAsync();
            if (d.PropertyType == typeof(bool))
            {
                var cell = new SwitchCell();
                cell.Text = d.Label;
                cell.On = val;
                cell.BindingContext = d;
                cell.OnChanged += Property_ValueChanged_Bool;
                return cell;
            }
            else if (d.PropertyType == typeof(double) || d.PropertyType == typeof(int))
            {
                if (d.MaxValue != null && d.MinValue != null)
                {
                    if (d.PropertyType == typeof(double))
                    {
                        SliderCell sc = new SliderCell(d.Label, double.Parse(d.MaxValue), double.Parse(d.MinValue), val);
                        sc.BindingContext = d;
                        sc.ValueChanged += Property_ValueChanged<double>;
                        return sc;
                    }
                    else if (d.PropertyType == typeof(int))
                    {
                        SliderCell sc = new SliderCell(d.Label, int.Parse(d.MaxValue), int.Parse(d.MinValue), val);
                        sc.BindingContext = d;
                        sc.ValueChanged += Property_ValueChanged<int>;
                        return sc;
                    }
                }
                else
                {
                    var cell = new ViewCell();
                    Button inc = new Button()
                    {
                        Text = "+"
                    };
                    Button dec = new Button()
                    {
                        Text = "-"
                    };
                    inc.Pressed += IncrementValue;
                    dec.Pressed += DecrementValue;
                }

            }
            return null;
        }

        private void DecrementValue(object sender, EventArgs e)
        {
            Button b = sender as Button;
            //b.Parent.Val
        }

        private void IncrementValue(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void Property_ValueChanged_Bool(object sender, ToggledEventArgs e)
        {
            if (timeSinceLastRequest + new TimeSpan(0, 0, 1) > DateTime.Now) return;
            timeSinceLastRequest = DateTime.Now;
            Cell v = (Cell)sender;
            DeviceProperty da = (DeviceProperty)v.BindingContext;
            await da.SetValueAsync(e.Value);
        }
        DateTime timeSinceLastRequest = DateTime.Now;
        private async void Property_ValueChanged<T>(object sender, ValueChangedEventArgs e)
        {
            if (timeSinceLastRequest + new TimeSpan(0, 0, 1) > DateTime.Now) return;
            timeSinceLastRequest = DateTime.Now;
            Cell v = (Cell)sender;
            DeviceProperty da = (DeviceProperty)v.BindingContext;
            await da.SetValueAsync(Convert.ChangeType(e.NewValue, typeof(T)));
        }
    }
}
