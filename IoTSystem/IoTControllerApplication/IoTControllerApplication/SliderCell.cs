using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace IoTControllerApplication
{
    public class SliderCell<T> : ViewCell
    {
        public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs<T> e);
        public event ValueChangedEventHandler ValueChanged;

        private T _value;
        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                ValueChangedEventArgs<T> v = new ValueChangedEventArgs<T>(_value, value);
                slider.Value = Convert.ToDouble(value);
                valueLabel.Text = _value.ToString();
                _value = value;
                ValueChanged(this, v);
            }
        }
        private Slider slider;
        private Label valueLabel;
        public SliderCell(string label, T maximum, T minimum, T defaultValue)
        {
            slider = new Slider()
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Maximum = Convert.ToDouble(maximum),
                Minimum = Convert.ToDouble(minimum),
                Value = Convert.ToDouble(defaultValue)
            };
            _value = defaultValue;
            valueLabel = new Label()
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Text = Value.ToString(),
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
                        Text = label,
                        WidthRequest = 64
                    },
                    slider,
                    valueLabel
                }
            };
        }
    }

    public class ValueChangedEventArgs<T>
    {
        public T OldValue { get; }

        public T NewValue { get; }

        public ValueChangedEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

    }
}
