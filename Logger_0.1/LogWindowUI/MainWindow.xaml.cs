using FabricationLogger.Buisness;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace FabricationLogger.LogWindowUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public ObservableCollection<LogEntry> LogEntries;
        public ObservableCollection<LogSystem> LogSystems;
        public ObservableCollection<LogLevel> LogLevels;

        private ICollectionView _filteredLogEntries;
        private bool _autoScrollEnabled = false;

        public bool IsAutoScrollEnabled { get; set; }
        public string FilePath { get; set; }


        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
            LogEntries = new ObservableCollection<LogEntry>();
            LogSystems = new ObservableCollection<LogSystem>();
            LogLevels = new ObservableCollection<LogLevel>();
            _filteredLogEntries = CollectionViewSource.GetDefaultView(LogEntries);
            _filteredLogEntries.Filter = LogEntriesFilterPredicate;

            _filteredLogEntries.CollectionChanged += OnLogEntriesChangedScrollToBottom;

            // Data binding
            LogEntryList.ItemsSource = LogEntries;
            Systems.ItemsSource = LogSystems;
            Levels.ItemsSource = LogLevels;

            FileHeaderText.DataContext = CSVPublisher.Instance;
            FileNameTextBox.DataContext = CSVPublisher.Instance;
            FilePathTextBox.DataContext = CSVPublisher.Instance;
            PublishStatusText.DataContext = CSVPublisher.Instance;

            PortBox.DataContext = UDPListener.Instance;
            IPAddressBox.DataContext = UDPListener.Instance;
            NetworkStatusText.DataContext = UDPListener.Instance;
        }

        /// <summary>
        /// Filtering the vizualized UI List
        /// </summary>
        private bool LogEntriesFilterPredicate(object item)
        {
            LogEntry entry = item as LogEntry;
            if (LogSystems.Any(s => s.Name == entry.System && !s.Enabled))
            {
                return false;
            }

            if (LogLevels.Any(s => s.Name == entry.Level && !s.Enabled))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// for autoscroll feature
        /// </summary>
        private void OnLogEntriesChangedScrollToBottom(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!IsAutoScrollEnabled)
            {
                return;
            }
    
            if (VisualTreeHelper.GetChildrenCount(LogEntryList) > 0)
            {
                Decorator border = VisualTreeHelper.GetChild(LogEntryList, 0) as Decorator;
                ScrollViewer scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
                scrollViewer.ScrollToBottom();
            }
        }

        /// <summary>
        /// Creates different System Tags for UI Bindings
        /// </summary>
        public void ConfigureSystems(List<string> systems)
        {
           
            systems.ForEach((system) =>
            {
                LogSystem entry = new LogSystem
                {
                    Name = system,
                    Enabled = true
                };
                entry.PropertyChanged += OnSystemEnableChanged;
                LogSystems.Add(entry);
                Console.Write("\n New Log system created : " + system);
            });

        }

        private void OnSystemEnableChanged(object sender, PropertyChangedEventArgs args)
        {
            _filteredLogEntries.Refresh();
        }

        /// <summary>
        /// Creats different severity level and log style for UI Bindings
        /// </summary>
        public void ConfigureLevels(List<Tuple<string, string>> levels)
        {
            // create log levels
            for (int i = 0; i < levels.Count; ++i)
            {
                LogLevel entry = new LogLevel
                {
                    Severity = i,
                    Name = levels[i].Item1,
                    Enabled = true,
                    Color = (Brush)new BrushConverter().ConvertFromString(levels[i].Item2)
                };
                entry.PropertyChanged += OnLevelSelectedChanged;
                LogLevels.Add(entry);
            }

            // style ListView based on the data from the log levels
            Style logListStyle = new Style
            {
                TargetType = typeof(ListViewItem)
            };
            foreach (LogLevel level in LogLevels)
            {
                DataTrigger trigger = new DataTrigger();
                trigger.Binding = new Binding("Level");
                trigger.Value = level.Name;
                trigger.Setters.Add(new Setter(ListViewItem.ForegroundProperty, level.Color));

                logListStyle.Triggers.Add(trigger);
            }

            LogEntryList.ItemContainerStyle = logListStyle;
        }

        private void OnLevelSelectedChanged(object sender, PropertyChangedEventArgs args)
        {
            _filteredLogEntries.Refresh();
        }

        public void AddLogEntry(LogEntry logEntry)
        {
            LogEntries.Add(logEntry);
            //Console.Write("\n UI new Log added");
        }


        ///////////// UI TRIGGERS

        private void LogButton_Click(object sender, RoutedEventArgs e)
        {
            Button logButton = sender as Button;

            if (UDPListener.Instance.IsEnabled())
            {
                // stop listening
                UDPListener.Instance.Pause();
                PortBox.IsEnabled = true;
                IPAddressBox.IsEnabled = true;
                logButton.Content = "START LISTENING";
                logButton.Background = Brushes.SteelBlue;
            }
            else
            {
                // start listening
                try
                {
                    UDPListener.Instance.Start();
                    PortBox.IsEnabled = false;
                    IPAddressBox.IsEnabled = false;
                    logButton.Content = "STOP LISTENING";
                    logButton.Background = Brushes.OrangeRed;
                }
                catch (Exception ex)
                {
                    Console.Write(" Error attempting to start Listener : " + ex);
                }
            }
        }

        private void PublishToFile_Click(object sender, RoutedEventArgs e)
        {
            CSVPublisher.Instance.Publish();
        }
    }
}
