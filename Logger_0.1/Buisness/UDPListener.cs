using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JamesHayward.Utilities;

/// <Notes>
/// Problem - Proper closure of Socket currently not working
/// Meaning that a socket can not be choosen twice. this
/// needs to be mentioned in the window for the user also.
/// </Notes>
namespace FabricationLogger.Buisness
{
    

    class UDPListener : Singleton<UDPListener>, ILogSubmitter, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ILog _log;
        private string _messageText = "";
        private DateTime _messageTimeStamp;
        private string _ip = "192.168.0.104";
        private int _listenPort = 30002;
        private int _port;

        private UdpClient _udpClient;
        private bool _go = false;
        private bool _begunReceiving = true;
        private bool _messageReceived = false;
        private string _debugLog = "";

        public bool IsEnabled() { return _go; }

        private void NotifyPropertyChange([CallerMemberName] string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

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

        public string IP
        {
            get
            {
                return _ip;
            }
            set
            {
                _ip = value;
                if (PropertyChanged != null)
                {
                    NotifyPropertyChange();
                }
            }
        }

        public int Port
        {
            get
            {
                return _listenPort;
            }
            set
            {
                _listenPort = value;
                if (PropertyChanged != null)
                {
                    NotifyPropertyChange();
                }
            }
        }

        public string Message
        {
            get
            {
                return _messageText;
            }
            set
            {
                _messageText = value;
                if (PropertyChanged != null)
                {
                    SubmitLog();
                }
            }
        }

        public void SubmitLog()
        {
            if (_log != null)
                _log.SubmitLog(_messageTimeStamp, _messageText);
            else
                Console.Write("\n Log Submitter has no log to submit to");
        }

        public void SetLog(ILog newLog)
        {
            _log = newLog;
        }

        public void Start()
        {
            if (_udpClient == null)
            {
                SetUP();
            }
            else if(_port != _listenPort)
            {
                StopReceiving();
                UpdateAddress();
            }
            _go = true;
            Tick();
        }

        private void SetUP()
        {
            try
            {
                IPEndPoint remoteIP = new IPEndPoint(IPAddress.Parse(_ip), _listenPort);
                _udpClient = new UdpClient(remoteIP);
            }
            catch (Exception e)
            {
                DebugLog = ("\n Error setting up Socket : " + e);
            }
        }

        private void UpdateAddress()
        {    
            try
            {
                _port = _listenPort;
                IPEndPoint remoteIP = new IPEndPoint(IPAddress.Parse(_ip), _port);
                _udpClient = new UdpClient(remoteIP);
            }
            catch (Exception ex)
            {
                DebugLog = ("\n Error in binding new Socket : " + ex.Message);
                DebugLog = ("\n Check your local IP address and port and try again");
            }
        }

        public void Pause()
        {
            _go = false;
            DebugLog = ("\n UDP Receiver paused");
        }

        private void Tick()
        {
            //If we already had started receiving
            if (_begunReceiving)
            {
                if (_messageReceived)
                {
                    //We set that message was collected
                    _messageReceived = false;
                    Console.Write("\n Receiver : Value is updated...");
                }
                else
                {
                    Console.Write("\n Receiver : Loop with no updates...");
                }

                if (_go == false)
                {
                    StopReceiving();
                }
                else
                {
                    if (ReceiveMessages(_port))
                    {
                        //port = _listenPort;
                    }
                }
            }
            //Otherwise we need to start receiving...
            else
            {
                if (_go)
                {
                    _port = _listenPort;
                    ReceiveMessages(_port);
                }
                else
                {
                    DebugLog = ("\n Receiver : Waiting for a port to listen to...");
                }
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                IPEndPoint remoteIP = new IPEndPoint(IPAddress.Parse(_ip), 0);
                byte[] receiveBytes = _udpClient.EndReceive(ar, ref remoteIP);
                _messageTimeStamp = DateTime.Now;
                Message = Encoding.UTF8.GetString(receiveBytes);
                Console.Write($"\n Server received this msg on {remoteIP.Port}: {_messageText}");
                _messageReceived = true;
                _udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
            }
            catch (Exception ex)
            {
                Console.Write("\n Error in ReceiveCallback: " + ex.Message);
            }
        }

        private bool ReceiveMessages(int port)
        {
            try
            {
                DebugLog = ($"\n Listening for messages on : " + _udpClient.Client.LocalEndPoint);
                _udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);

                _begunReceiving = true;
                return true;
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                if (ex.SocketErrorCode == System.Net.Sockets.SocketError.AddressAlreadyInUse)
                {
                    DebugLog = ("\n Socket error: " + ex.Message + ".");
                }
            }
            catch (Exception ex)
            {
                DebugLog = ("\n Error: " + ex.Message);
            }

            return false;
        }

        private void StopReceiving()
        {
            try
            {
                if (_udpClient != null)
                {
                    _udpClient.Client.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                    _udpClient.Close();
                    _udpClient.Dispose();
                    _udpClient = null;
                }
                _begunReceiving = false;
                _go = false;
                DebugLog = ("\n Stopped receiving");
            }
            catch (Exception ex)
            {
                DebugLog = ("\n Error while stopping receiving: " + ex.Message);
            }
        }

    }
}
