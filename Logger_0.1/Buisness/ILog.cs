using System;
using System.Collections.ObjectModel;


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
