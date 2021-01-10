using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabricationLogger.Buisness
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public string Message { get; }
        public MessageReceivedEventArgs(string message)
        {
            Message = message;
        }
    }

    public class LogEnteredEventArgs : EventArgs
    {
        public LogEntry NewEntry { get; }
        public LogEnteredEventArgs(LogEntry newEntry)
        {
            NewEntry = newEntry;
        }
    }
}
