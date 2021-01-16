using System;

namespace FabricationLogger.Buisness
{
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string System { get; set; }
        public string Message { get; set; }
        public string Level { get; set; }
    }
}
