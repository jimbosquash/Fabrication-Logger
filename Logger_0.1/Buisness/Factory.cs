using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabricationLogger.Buisness
{
    class Factory
    {
        public static ILogPublisher CreateNewPublisher(ILog log)
        {
            return new CSVPublisher(log);
        }

        public static ListenerBase CreateNewListener(ILog log)
        {
            var listener = new UDPListener();
            listener.SetLog(log);
            return listener;
        }
    }
}
