using System;
using System.Collections.ObjectModel;
using Utilities;

namespace FabricationLogger.Buisness
{
    /// <summary>
    /// Responsible for holding and editing log objects
    /// </summary>
    class Log : Singleton<Log>, ILog
    {
        protected ObservableCollection<LogEntry> _logEntries = new ObservableCollection<LogEntry>();

        public event EventHandler<LogEnteredEventArgs> LogEnteredEventHandler;

        public ObservableCollection<LogEntry> GetLogEntries()
        {
            return Instance._logEntries;
        }

        private static void AddLogEntry(LogEntry newLog)
        {
            Instance._logEntries.Add(newLog);
            Console.Write("\n New Log : " + newLog.Timestamp + " : " + newLog.System + " : " + newLog.Message);
            LogEnteredEventArgs arg = new LogEnteredEventArgs(newLog);
            Instance.LogEnteredEventHandler?.Invoke(Instance, arg);
        }

        public void SubmitLog(DateTime timeStamp, string message)
        {
            AddLogEntry(CreateLogEntry(timeStamp, message));
        }

        private static LogEntry CreateLogEntry(DateTime timeStamp, string message)
        {
            LogEntry var = new LogEntry
            {
                Timestamp = timeStamp,
                Message = message,
                System = "UNKNOWN",
                Level = ""

            };
            Console.Write("\n New Log : " + var.Timestamp + " : " + var.Message);
            return var;
        }

        public void SubmitLog(DateTime timeStamp, string logLevel, string system, string message)
        {
            AddLogEntry(CreateLogEntry(timeStamp, logLevel, system, message));
        }

        private LogEntry CreateLogEntry(DateTime timeStamp, string logLevel, string system, string message)
        {
            LogEntry newlogEntry = new LogEntry
            {
                Timestamp = timeStamp,
                System = system,
                Message = message,
                Level = logLevel
            };
            return newlogEntry;
        }

    }
}
