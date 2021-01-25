using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Threading;
using FabricationLogger.LogWindowUI;
using FabricationLogger.Buisness;
using Utilities;


namespace FabricationLogger
{
    /// <summary>
    /// LoggerUI is the View-Model responsible for initialting, configuring
    /// and updating GUI. Currently Configurations are hard coded into the 
    /// View-Model on client request.
    /// </summary>
    public sealed class LoggerUI : Singleton<LoggerUI>
    {
        private App m_application;
        private ILog _log;
        public ListenerBase Listener;
        public event EventHandler<LogEnteredEventArgs> OnNewLogEntered;

        public void SetListener(ListenerBase listener)
        {
            Listener = listener;

            m_application.Dispatcher.BeginInvoke((Action)delegate
            {
                Debug.Assert(m_application.MainWindow != null);

                (m_application.MainWindow as MainWindow).Listener = Listener;
            });
        }

        private void OnLogEntered(object sender, LogEnteredEventArgs e)
        {
            Add(e.NewEntry);
        }

        public void CreateApp(Rect dimensions)
        {            
            CreateUIApp(dimensions);
            ConfigureUILogTags();
            
        }

        /// <summary>
        /// could be thought of as setting models
        /// </summary>
        public void SetDependencies(ILog log,ILogPublisher publisher, ListenerBase listener)
        {
            SetLog(log);
            BindToListener(listener);
            BindToPublisher(publisher);
        }

        public void SetLog(ILog log)
        {
            _log = log;
            _log.LogEnteredEventHandler += OnLogEntered;
        }

        private void ConfigureUILogTags()
        {
            Instance.ConfigureSytems(new List<string>
            {
                "UNKNOWN",// 
                "TEST",
                "ROBOT",
                "SENSOR 1",
                "SENSOR 2"
            });

            Instance.ConfigureLevels(new List<Tuple<string, string>>
            {
                Tuple.Create("DEBUG",   "#000000"),
                Tuple.Create("WARNING", "#B8860B"),
                Tuple.Create("ERROR",   "#FF0000")
            });
        }

        private void CreateUIApp(Rect dimensions)
        {
            // application and window need their own thread, this creates that
            AutoResetEvent windowCreatedEvent = new AutoResetEvent(false);

            Thread t = new Thread(() =>
            {
                m_application = new App();
                MainWindow window = new MainWindow()
                {
                    // set window dimensions
                    WindowStartupLocation = WindowStartupLocation.Manual,
                    Left = dimensions.Left,
                    Top = dimensions.Top,
                    Width = dimensions.Width,
                    Height = dimensions.Height
                };

                m_application.MainWindow = window;
                m_application.MainWindow.Show();

                //notify they are created before we block this thread
                windowCreatedEvent.Set();

                m_application.Run();

            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            // wait until the application and window are created
            windowCreatedEvent.WaitOne();
        }
        
        public void BindToListener(ListenerBase listener)
        {
            Debug.Assert(m_application != null);

            m_application.Dispatcher.BeginInvoke((Action)delegate
            {
                (m_application.MainWindow as MainWindow).BindToListener(listener);
            });
        }

        public void BindToPublisher(ILogPublisher publisher)
        {
            Debug.Assert(m_application != null);

            m_application.Dispatcher.BeginInvoke((Action)delegate
            {
                (m_application.MainWindow as MainWindow).BindToPublisher(publisher);
            });
        }
        public void Add(LogEntry logEntry)
        {
            Debug.Assert(m_application != null);

            m_application.Dispatcher.BeginInvoke((Action)delegate
            {
                if (m_application.MainWindow == null)
                {
                    return;
                }
                (m_application.MainWindow as MainWindow).AddLogEntry(logEntry);
            });
        }

        private void ConfigureSytems(List<string> systems)
        {
            Debug.Assert(m_application != null);

            m_application.Dispatcher.BeginInvoke((Action)delegate
            {
                Debug.Assert(m_application.MainWindow != null);

                (m_application.MainWindow as MainWindow).ConfigureSystems(systems);
            });
        }

        private void ConfigureLevels(List<Tuple<string, string>> levels)
        {
            Debug.Assert(m_application != null);

            m_application.Dispatcher.BeginInvoke((Action)delegate
            {
                Debug.Assert(m_application.MainWindow != null);

                (m_application.MainWindow as MainWindow).ConfigureLevels(levels);
            });
        }
    }
}

