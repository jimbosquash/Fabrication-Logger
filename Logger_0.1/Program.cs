using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FabricationLogger.LogWindowUI;
using FabricationLogger.Buisness;

namespace FabricationLogger
{
    /// <summary>
    /// This program is currently set up for a single entry point 
    /// by using a singleton pattern. Future iterates can change
    /// this as needed. 
    /// 
    /// The program will create a log, Listener, publisher, and 
    /// required UI through a View-Model class "LoggerUI" and 
    /// then perform Unit testing of the core system. 
    /// </summary>

    class Program
    {
        static void Main(string[] args)
        {
            Log FabricationLog = Log.Instance;

            // intialization step
            UDPListener.Instance.SetLog(FabricationLog);
            CSVPublisher.Instance.SetLog(FabricationLog);
            LoggerUI.Instance.CreateApp(new System.Windows.Rect(0, 0, 1000, 600));
            LoggerUI.Instance.SetLog(FabricationLog);

            // <Test Zone>
            UnitTester.RunUnitTests();
            // <\Test Zone End>
        }
    }

}
