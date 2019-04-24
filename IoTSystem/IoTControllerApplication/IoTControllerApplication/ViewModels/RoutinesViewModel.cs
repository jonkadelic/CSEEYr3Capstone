using IoTControllerApplication.Models;
using IoTControllerApplication.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IoTControllerApplication.ViewModels
{
    public class RoutinesViewModel : BaseViewModel
    {
        public ObservableCollection<Routine> Routines { get; set; }

        public Command LoadItemsCommand { get; set; }
        public RoutinesViewModel()
        {
            Title = "Routines";
            Routines = new ObservableCollection<Routine>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var routines = (await RoutineDataStore.DataStore.GetItemsAsync(true)).ToList();
                routines.Sort((x, y) => x.Name.CompareTo(y.Name));
                Routines.Clear();
                foreach (Routine r in routines)
                {
                    Routines.Add(r);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
