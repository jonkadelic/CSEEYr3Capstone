using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using IoTControllerApplication.Models;
using IoTControllerApplication.ViewModels;

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

            foreach (Cell cell in viewModel.DeviceAttributeCells)
            {
                AttributesTableView.Root[0].Add(cell);
            }
        }
    }
}