using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Utilities;


namespace FabricationLogger.Buisness
{
    class CSVPublisher : Singleton<CSVPublisher>,ILogPublisher,INotifyPropertyChanged
    {
        private ILog _log;
        private string _path = "D:\\";
        private string _fileName = DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString() + DateTime.Today.Day.ToString() + "_ChangeFileName_0" ;
        private string _fileHeader = DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString() + DateTime.Today.Day.ToString() +"_header of the file " +
                                                                                                                                       "change in Log Publisher class";
        private string _fullFilePath = "";
        private string _debugLog = "";

        public event PropertyChangedEventHandler PropertyChanged;

        public string DebugLog
        {
            get
            {
                return _debugLog;
            }
            set
            {
                _debugLog = value;
                Console.Write("\n" + _debugLog);
                NotifyPropertyChange();
            }
        }

        public string FilePath
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                NotifyPropertyChange();
            }
        }

        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
                NotifyPropertyChange();
            }
        }

        public string FileHeader
        {
            get
            {
                return _fileHeader;
            }
            set
            {
                _fileHeader = value;
                NotifyPropertyChange();
            }
        }

        public void SetLog(ILog newLog)
        {
            _log = newLog;
        }

        private void NotifyPropertyChange([CallerMemberName] string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
            //Console.Write("\n Log Publisher Property Changed event triggered");
        }

        public void Publish()
        {
            _fullFilePath = GetFilePath();

            if (CheckFileNameAvailability())
            {
                CreateFile();
            }        
        }

        public string GetFilePath()
        {
            _fullFilePath = _path + _fileName + ".csv";
            return _fullFilePath;
        }
        private string GetFileContent()
        {
            StringBuilder csvContent = new StringBuilder();

            foreach (LogEntry log in _log.GetLogEntries())
            {
                string newLine = ConvertLogToString(log);
                csvContent.AppendLine(newLine);
            }
            string grid = "\n" + "Time stamp, System, Message " + "\n";
            csvContent.Insert(0, grid);
            csvContent.Insert(0, _fileHeader);
            return csvContent.ToString();
        }

        private bool CheckFileNameAvailability()
        {
            //Appends '_0' onto file
            int i = 0;
            while (File.Exists(_fullFilePath))
            {
                i++;
                string[] var = FileName.Split('_');
                List<string> varList = var.ToList();
                if(varList.Count != 1)
                    varList.RemoveAt(var.Length - 1); // this depends on input name ending with '_x'
                FileName = string.Join("_", varList);
                FileName = FileName + "_" + i;
                _fullFilePath = GetFilePath();
            }
            return CheckifPathValid();
        }

        private bool CheckifPathValid()
        {
            try
            {
                Path.GetFullPath(_fullFilePath);
                DebugLog = ("\n Path accepted : " + _fullFilePath);
                return true;
            }
            catch
            {
                DebugLog = ("\n Failed to Generate File : " + _fullFilePath + " : Remove any characters : ,< ,> ,: ," + "/ ," + "| ," + "? ," + " *.");
                return false;
            }
        }

        private void CreateFile()
        {
            try
            {
                File.AppendAllText(_fullFilePath, GetFileContent());
                DebugLog = "\n File Created : " + _fullFilePath;
            }
            catch (Exception e)
            {
                DebugLog = "\n Error in saving File : " + e;
            }
        }

        private string ConvertLogToString(LogEntry logEntry)
        {
            string newEntry = logEntry.Timestamp.ToString() + ','
                            + logEntry.System + ','
                            + logEntry.Message;
            return newEntry;
        }

        
    }
}
