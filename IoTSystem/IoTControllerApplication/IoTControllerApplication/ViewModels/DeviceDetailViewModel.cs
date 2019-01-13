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
                        cell.OnChanged += Switch_OnChanged;
                        cells.Add(cell);
                    }
                    else if (d.AttributeType == typeof(double) || d.AttributeType == typeof(int))
                    {
                        var cell = new ViewCell();
                        Slider s = new Slider()
                        {
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Maximum = Convert.ChangeType(d.MaxValue, typeof(double)),
                            Minimum = Convert.ChangeType(d.MinValue, typeof(double)),
                            Value = d.Value,
                        };
                        s.BindingContext = d;
                        if (d.AttributeType == typeof(int))
                        {
                            s.ValueChanged += Slider_ValueChangedInt;
                        }
                        else
                        {
                            s.ValueChanged += Slider_ValueChanged;
                        }
                        Label l = new Label()
                        {
                            Text = d.Value.ToString(),
                            WidthRequest = 32,
                            BindingContext = s
                        };
                        l.SetBinding(Label.TextProperty, new Binding("Value"));
                        cell.View = new StackLayout()
                        {
                            Orientation = StackOrientation.Horizontal,
                            Margin = new Thickness(16, 8),
                            Children =
                            {
                                new Label()
                                {
                                    Text = d.Label,
                                    WidthRequest = 64
                                },
                                s,
                                l
                            }
                        };
                        cells.Add(cell);
                    }
                }
                return cells;
            }

        }

        private void Switch_OnChanged(object sender, ToggledEventArgs e)
        {
            Cell v = (Cell)sender;
            DeviceAttribute da = (DeviceAttribute)v.BindingContext;
            da.Value = e.Value;
        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            View v = (View)sender;
            DeviceAttribute da = (DeviceAttribute)v.BindingContext;
            da.Value = e.NewValue;
        }

        private void Slider_ValueChangedInt(object sender, ValueChangedEventArgs e)
        {
            View v = (View)sender;
            DeviceAttribute da = (DeviceAttribute)v.BindingContext;
            da.Value = (int) e.NewValue;
        }
    }
}
