using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace IoTControllerApplication
{
    public class SliderCell : ViewCell
    {
        public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs e);
        public event ValueChangedEventHandler ValueChanged;

        private double _value;
        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                ValueChangedEventArgs v = new ValueChangedEventArgs(_value, value);
                slider.Value = value;
                _value = value;
                valueLabel.Text = LabelValue;
                ValueChanged(this, v);
            }
        }
        private Slider slider;
        private Label valueLabel;
        public SliderCell(string label, double maximum, double minimum, double defaultValue)
        {
            slider = new Slider()
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Maximum = maximum,
                Minimum = minimum,
                Value = defaultValue,
                MinimumTrackColor = Color.Accent
            };
            _value = defaultValue;
            slider.SetBinding(Slider.ValueProperty, new Binding("Value", source: this));
            valueLabel = new Label()
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Text = LabelValue,
                WidthRequest = 32
            };
            View = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Margin = new Thickness(16, 8),
                Children =
                {
                    new Label()
                    {
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Text = label,
                        WidthRequest = 64
                    },
                    slider,
                    valueLabel
                }
            };
        }

        private string LabelValue {
            get
            {
                if (slider.Minimum == 0.0 && slider.Maximum == 1.0)
                    return ((int)(Value * 100)).ToString() + "%";
                else
                    return ((int)Value).ToString();
            }
        }
    }

    public class ValueChangedEventArgs
    {
        public double OldValue { get; }

        public double NewValue { get; }

        public ValueChangedEventArgs(double oldValue, double newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

    }
}
