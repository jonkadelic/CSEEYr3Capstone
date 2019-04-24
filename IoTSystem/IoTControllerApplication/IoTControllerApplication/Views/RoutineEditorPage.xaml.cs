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
    public partial class RoutineEditorPage : ContentPage
    {
        RoutineEditorViewModel viewModel;
        ToolbarItem DeleteButton;
        public RoutineEditorPage(RoutineEditorViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;

            if (viewModel.modifying)
            {
                DevicePicker.SelectedIndex = viewModel.DevicesList.IndexOf(viewModel.TargetDevice.Label);
                PropertyPicker.SelectedIndex = viewModel.PropertiesList.IndexOf(viewModel.TargetProperty.Label);
                DeleteButton = new ToolbarItem();
                DeleteButton.Text = "Delete";
                DeleteButton.Clicked += DeleteButton_Clicked;

                ToolbarItems.Add(DeleteButton);

                foreach(RoutineCondition rc in viewModel.routine.Conditions)
                {
                    MainStackLayout.Children.Add(new RoutineConditionView(new RoutineConditionViewModel(rc)));
                }
            }
        }

        private void DevicePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            IoTDevice device = DeviceDataStore.DataStore.GetItemsAsync().Result.Where(x => x.IsReadOnly == false).ToList()[DevicePicker.SelectedIndex];
            viewModel.TargetDevice = device;
            PropertyPicker.ItemsSource = null;
            PropertyPicker.ItemsSource = viewModel.PropertiesList;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            viewModel.routine.Conditions = MainStackLayout.Children.Where(x => x is RoutineConditionView).Select(x => (x as RoutineConditionView).viewModel.condition).ToList();
            if (viewModel.modifying) await RoutineDataStore.DataStore.DeleteItemAsync(viewModel.routine.ID);
            await RoutineDataStore.DataStore.AddItemAsync(viewModel.routine);
            await Navigation.PopAsync();
        }

        private void PropertyPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DeviceProperty property = viewModel.TargetDevice.Properties[PropertyPicker.SelectedIndex];
                viewModel.TargetProperty = property;
            }
            catch (Exception)
            {
                viewModel.TargetProperty = null;
            }
        }

        private void AddConditionButton_Clicked(object sender, EventArgs e)
        {
            MainStackLayout.Children.Add(new RoutineConditionView(new RoutineConditionViewModel(new RoutineCondition())));
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            await RoutineDataStore.DataStore.DeleteItemAsync(viewModel.routine.ID);
            await Navigation.PopAsync();
        }
    }
}