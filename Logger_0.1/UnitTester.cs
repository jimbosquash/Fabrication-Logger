using FabricationLogger.Buisness;
using System;
using System.Diagnostics;
using System.IO;

namespace FabricationLogger
{
    class UnitTester
    {
        /// <summary>
        /// Testing codes, please disable them once debuging checked
        /// </summary>
        /// 
        public static void RunUnitTests(ListenerBase listener, ILogPublisher publisher)
        {
            TestAddLogEntries(10);
            TestListenerUpdate(listener,"192.168.0.38", 30001);
            TestPublishLogsToFile(publisher,"testFile", "D:\\");
        }

        private static void TestAddLogEntries(int count)
        {
            for (int i = 0; i < count; i++)
            {
                TestAddLogEntry();
            }
        }

        private static void TestAddLogEntry()
        {
            Log.Instance.SubmitLog(DateTime.Now, "DEBUG", "TEST", "This is a test log container message");
            Log.Instance.SubmitLog(DateTime.Now, "WARNING", "TEST", "This is a test log container message");
            Log.Instance.SubmitLog(DateTime.Now, "ERROR", "TEST", "This is a test log container message");

        }

        private static void TestListenerUpdate(ListenerBase listener,string ip, int port)
        {
            // start listening
            // change port
            // check port changed
            listener.Port = port;
            listener.IP = ip;
            listener.Start();
            Console.Write("\n Test Updated Address : " + listener.Port);
            listener.Pause();
        }

        private static void TestPublishLogsToFile(ILogPublisher publisher,string fileName, string path)
        {
            publisher.FileName = fileName;
            publisher.FilePath = path;
            publisher.Publish();
            string testPath = publisher.GetFilePath();
            Debug.Assert(File.Exists(testPath),"\n Test : Failed to Generate requested file");

        }
    }
}
