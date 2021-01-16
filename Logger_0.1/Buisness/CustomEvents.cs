using System;

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
