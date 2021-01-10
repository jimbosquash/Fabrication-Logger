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

        public static void RunUnitTests()
        {
            TestAddLogEntries(10);
            TestListenerUpdate("192.168.0.38", 30001);
            TestPublishLogsToFile("testFile", "D:\\");
        }

        static public void TestAddLogEntries(int count)
        {
            for (int i = 0; i < count; i++)
            {
                TestAddLogEntry();
            }
        }

        static private void TestAddLogEntry()
        {
            Log.Instance.SubmitLog(DateTime.Now, "DEBUG", "TEST", "This is a test log container message");
            Log.Instance.SubmitLog(DateTime.Now, "WARNING", "TEST", "This is a test log container message");
            Log.Instance.SubmitLog(DateTime.Now, "ERROR", "TEST", "This is a test log container message");

        }

        static private void TestListenerUpdate(string ip, int port)
        {
            // start listening
            // change port
            // check port changed
            // 
            UDPListener.Instance.Port = port;
            UDPListener.Instance.IP = ip;
            UDPListener.Instance.Start();
            Console.Write("\n Test Updated Address : " + UDPListener.Instance.Port);
            UDPListener.Instance.Pause();
        }

        static private void TestPublishLogsToFile(string fileName, string path)
        {
            CSVPublisher.Instance.FileName = fileName;
            CSVPublisher.Instance.FilePath = path;
            CSVPublisher.Instance.Publish();
            string testPath = CSVPublisher.Instance.GetFilePath();
            Debug.Assert(File.Exists(testPath),"\n Test : Failed to Generate requested file");

        }
    }
}
