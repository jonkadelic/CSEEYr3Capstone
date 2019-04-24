using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DemonstrationVirtualDevice
{
    public class AppViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool PoweredValue { get; private set; } = false;
        public int PowerLevelValue { get; private set; } = 0;

        public string PowerButtonText
        {
            get
            {
                return PoweredValue ? "ON" : "OFF";
            }
        }

        public string PowerLevelText
        {
            get
            {
                return PowerLevelValue.ToString();
            }
        }


        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void TogglePowerButton()
        {
            PoweredValue = !PoweredValue;
            NotifyPropertyChanged("PowerButtonText");
        }

        public void IncrementPowerLevel()
        {
            PowerLevelValue = (PowerLevelValue + 1) % 6;
            NotifyPropertyChanged("PowerLevelText");
        }
    }
}
