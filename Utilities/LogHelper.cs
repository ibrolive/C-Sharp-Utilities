using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Utilities
{
    public class LogHelper
    {
        public LogHelper()
        {
        }

        public static void Log(string logEvent, string message)
        {
            Console.WriteLine(logEvent + " : " + message);
            Trace.WriteLine(logEvent + " : " + message);
        }
    }
}


