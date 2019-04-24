using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DemonstrationVirtualDevice
{
    public class AppViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Color bulbColor = Colors.White;
        private bool bulbPowered = true;

        public SolidColorBrush BulbColor
        {
            get
            {
                if (bulbPowered)
                {
                    return new SolidColorBrush(bulbColor);
                }
                else return Brushes.Black;
            }
        }

        public bool BulbPowered
        {
            get
            {
                return bulbPowered;
            }
            set
            {
                bulbPowered = value;
                NotifyPropertyChanged("BulbColor");
            }
        }

        public byte BulbR
        {
            get
            {
                return bulbColor.R;
            }
            set
            {
                bulbColor.R = value;
                NotifyPropertyChanged("BulbColor");
            }
        }

        public byte BulbG
        {
            get
            {
                return bulbColor.G;
            }
            set
            {
                bulbColor.G = value;
                NotifyPropertyChanged("BulbColor");
            }
        }

        public byte BulbB
        {
            get
            {
                return bulbColor.B;
            }
            set
            {
                bulbColor.B = value;
                NotifyPropertyChanged("BulbColor");
            }
        }

        bool plugState = false;
        public bool PlugState
        {
            get
            {
                return plugState;
            }
            set
            {
                plugState = value;
                NotifyPropertyChanged("PlugImage");
            }
        }

        public string PlugImage
        {
            get
            {
                return PlugState ? "plug.png" : "plug-off.png";
            }
        }
    }
}
