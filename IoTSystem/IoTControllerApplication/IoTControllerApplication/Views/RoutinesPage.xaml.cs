using IoTControllerApplication.Models;
using IoTControllerApplication.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IoTControllerApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoutinesPage : ContentPage
    {
        RoutinesViewModel viewModel;

        public RoutinesPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new RoutinesViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var routine = args.SelectedItem as Routine;
            if (routine == null)
                return;

            await Navigation.PushAsync(new RoutineEditorPage(new RoutineEditorViewModel(routine)));

            // Manually deselect item.
            RoutinesListView.SelectedItem = null;
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RoutineEditorPage(new RoutineEditorViewModel()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.LoadItemsCommand.Execute(null);
        }
    }
}
