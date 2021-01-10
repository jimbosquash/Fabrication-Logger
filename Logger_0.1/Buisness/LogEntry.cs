﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
