using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabricationLogger.LogWindowUI
{
    public class LogSystem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Name { get; set; }
        private bool _enabled { get; set; }

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
