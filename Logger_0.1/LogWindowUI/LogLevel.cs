using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FabricationLogger.LogWindowUI
{
    public class LogLevel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Name { get; set; }
        public Brush Color { get; set; }
        public int Severity { get; set; }

        private bool _enabled;
       
        public bool Enabled
        {
            get
            {
                return _enabled;
            }

            set
            {
                _enabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Enabled"));
            }
        }
    }
}
