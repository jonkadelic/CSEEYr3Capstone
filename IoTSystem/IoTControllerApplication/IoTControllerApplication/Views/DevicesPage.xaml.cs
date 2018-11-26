using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using IoTControllerApplication.Models;
using IoTControllerApplication.Views;
using IoTControllerApplication.ViewModels;

namespace IoTControllerApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DevicesPage : ContentPage
    {
        DevicesViewModel viewModel;

        public DevicesPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new DevicesViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var device = args.SelectedItem as IoTDevice;
            if (device == null)
                return;

            await Navigation.PushAsync(new DeviceDetailPage(new DeviceDetailViewModel(device)));

            // Manually deselect item.
            DevicesListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Devices.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}