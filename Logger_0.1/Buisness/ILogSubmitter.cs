using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabricationLogger.Buisness
{
    interface ILogSubmitter
    {
        void SubmitLog();
        void SetLog(ILog log); 
    }
}
