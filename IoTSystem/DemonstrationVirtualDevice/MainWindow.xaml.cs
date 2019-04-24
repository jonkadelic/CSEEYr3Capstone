using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DemonstrationVirtualDevice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Startup();
        }

        public void Startup()
        {
            ClientBroadcastReceiver.Run();
            HttpResponder.Run(ViewModel);
        }

        private AppViewModel ViewModel
        {
            get
            {
                return (AppViewModel)DataContext;
            }
        }

        private void PoweredButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.TogglePowerButton();
        }

        private void PowerLevelButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.IncrementPowerLevel();
        }
    }
}
