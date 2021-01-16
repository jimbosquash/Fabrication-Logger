using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabricationLogger.Buisness
{
    public abstract class ListenerBase
    {
        public abstract bool IsEnabled();


        public abstract int Port {
            get;
            set;
        }

        public abstract string IP
        {
            get;
            set;
        }

        public abstract string Message
        {
            get;
            set;
        }

        public abstract string DebugLog
        {
            get;
            set;
        }

        public abstract void Start();
        public abstract void Pause();
        protected abstract void SetUp();
        protected abstract void Tick();

        protected abstract void UpdateAddress();
        protected abstract void StopReceiving();

    }
}
