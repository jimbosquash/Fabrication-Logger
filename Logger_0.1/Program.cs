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
        public static ILog Log;
        public static ListenerBase Listener;
        public static ILogPublisher Publisher;
        public static ILogSubmitter LogSubmitter;

        static void Main(string[] args)
        {
            // Create Models for MVVM pattern
            Log = Buisness.Log.Instance;
            Listener = Factory.CreateNewListener(Log);
            Publisher = Factory.CreateNewPublisher(Log);

            // Create Instance of View Model and then Create View 
            LoggerUI.Instance.CreateApp(new System.Windows.Rect(0, 0, 1000, 600));
            LoggerUI.Instance.SetDependencies(Log, Publisher, Listener);

            // <Test Zone>
            //UnitTester.RunUnitTests(Listener,Publisher);
            // <\Test Zone End>
        }
    }

}
