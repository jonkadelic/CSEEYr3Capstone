using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using IoTControllerApplication.Models;
using IoTControllerApplication.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IoTControllerApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeviceDetailPage : ContentPage
    {
        DeviceDetailViewModel viewModel;

        public DeviceDetailPage(DeviceDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
            indicator.BindingContext = this.viewModel;

            _ = Reload();
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Reload();
        }

        private async Task Reload()
        {
            if (IsBusy) return;
            IsBusy = true;
            indicator.IsRunning = true;
            indicator.IsVisible = true;
            PropertiesTableView.Root[0].Clear();
            List<Cell> cells = await viewModel.GetDevicePropertyCellsAsync();
            indicator.IsRunning = false;
            indicator.IsVisible = false;
            foreach (Cell cell in cells)
            {
                PropertiesTableView.Root[0].Add(cell);
            }
            IsBusy = false;

        }
    }
}