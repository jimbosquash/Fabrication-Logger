using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabricationLogger.Buisness
{
    public interface ILog
    {
        event EventHandler<LogEnteredEventArgs> LogEnteredEventHandler;
        void SubmitLog(DateTime timeStamp, string message);
        void SubmitLog(DateTime timeStamp, string logLevel, string system, string message);
        ObservableCollection<LogEntry> GetLogEntries();
    }
}
