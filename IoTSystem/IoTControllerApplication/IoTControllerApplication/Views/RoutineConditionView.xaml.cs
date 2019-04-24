using IoTControllerApplication.Models;
using IoTControllerApplication.Services;
using IoTControllerApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IoTControllerApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoutineConditionView : ContentView
    {
        public RoutineConditionViewModel viewModel;
        public RoutineConditionView(RoutineConditionViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;

            ComparisonPicker.Items.Add("is less than");
            ComparisonPicker.Items.Add("is equal to");
            ComparisonPicker.Items.Add("is greater than");

            if (viewModel.Device != null && viewModel.Property != null)
            {
                DevicePicker.SelectedIndex = viewModel.DevicesList.IndexOf(viewModel.Device.Label);
                PropertyPicker.SelectedIndex = viewModel.PropertiesList.IndexOf(viewModel.Property.Label);
                switch (viewModel.condition.Comparison)
                {
                    case RoutineCondition.COMPARISON.LESS:
                        ComparisonPicker.SelectedIndex = 0;
                        break;
                    case RoutineCondition.COMPARISON.EQUAL:
                        ComparisonPicker.SelectedIndex = 1;
                        break;
                    case RoutineCondition.COMPARISON.GREATER:
                        ComparisonPicker.SelectedIndex = 2;
                        break;
                }
            }
        }

        private void DevicePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            IoTDevice device = DeviceDataStore.DataStore.GetItemsAsync().Result.ToList()[DevicePicker.SelectedIndex];
            viewModel.Device = device;
            PropertyPicker.ItemsSource = null;
            PropertyPicker.ItemsSource = viewModel.PropertiesList;
        }

        private void PropertyPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DeviceProperty property = viewModel.Device.Properties[PropertyPicker.SelectedIndex];
                viewModel.Property = property;
            }
            catch (Exception)
            {
                viewModel.Property = null;
            }
        }

        private void ComparisonPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ComparisonPicker.SelectedIndex)
            {
                case 0:
                    viewModel.condition.Comparison = RoutineCondition.COMPARISON.LESS;
                    break;
                case 1:
                    viewModel.condition.Comparison = RoutineCondition.COMPARISON.EQUAL;
                    break;
                case 2:
                    viewModel.condition.Comparison = RoutineCondition.COMPARISON.GREATER;
                    break;
            }
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            (Parent as StackLayout).Children.Remove(this);
        }
    }
}