using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabricationLogger.Buisness
{
    interface ILogPublisher
    {
        void Publish();
        void SetLog(ILog log);
    }
}
