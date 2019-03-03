using System;
using System.Collections.Generic;
using IoTControllerApplication.Models;
using Xamarin.Forms;

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

        public List<Cell> DeviceAttributeCells
        {
            get
            {
                List<Cell> cells = new List<Cell>();
                foreach (DeviceAttribute d in Device.Attributes)
                {
                    d.UpdateValue();
                    if (d.AttributeType == typeof(bool))
                    {
                        var cell = new SwitchCell();
                        cell.Text = d.Label;
                        cell.On = d.Value;
                        cell.BindingContext = d;
                        cell.OnChanged += Attribute_ValueChanged_Bool;
                        cells.Add(cell);
                    }
                    else if (d.AttributeType == typeof(double) || d.AttributeType == typeof(int))
                    {
                        if (d.MaxValue != null && d.MinValue != null)
                        {
                            if (d.AttributeType == typeof(double))
                            {
                                SliderCell<double> sc = new SliderCell<double>(d.Label, d.MaxValue, d.MinValue, d.Value);
                                sc.BindingContext = d;
                                sc.ValueChanged += Attribute_ValueChanged;
                                cells.Add(sc);
                            }
                            else if (d.AttributeType == typeof(int))
                            {
                                SliderCell<int> sc = new SliderCell<int>(d.Label, d.MaxValue, d.MinValue, d.Value);
                                sc.BindingContext = d;
                                sc.ValueChanged += Attribute_ValueChanged;
                                cells.Add(sc);
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
                }
                return cells;
            }

        }

        private void DecrementValue(object sender, EventArgs e)
        {
            Button b = sender as Button;
            b.Parent
        }

        private void IncrementValue(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Attribute_ValueChanged_Bool(object sender, ToggledEventArgs e)
        {
            Cell v = (Cell)sender;
            DeviceAttribute da = (DeviceAttribute)v.BindingContext;
            da.Value = e.Value;
        }

        private void Attribute_ValueChanged<T>(object sender, ValueChangedEventArgs<T> e)
        {
            View v = (View)sender;
            DeviceAttribute da = (DeviceAttribute)v.BindingContext;
            da.Value = e.NewValue;
        }
    }
}
