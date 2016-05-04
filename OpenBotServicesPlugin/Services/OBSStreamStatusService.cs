using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.IO.Pipes;
using OpenBot.Plugins;

namespace OpenBotServicesPlugin.Services
{
    public class OBSStreamStatusService : AbstractService
    {
        private bool _isConnected;
        private bool _isStreaming;
        private string _pipeName = "OBSStreamStatusPlugin_OpenBot";
        private DateTime _streamStartTime;
        private DateTime _streamStopTime;
        NamedPipeServerStream _pipeServer;
        private event OnStreamStatusChanged _streamStatusChanged;
        public delegate void OnStreamStatusChanged(bool status);

        public bool IsConnected { get { return _isConnected; } }
        public bool Streaming
        {
            get
            {
                return _isStreaming;
            }
        }

        public DateTime StartTime
        {
            get
            {
                return _streamStartTime;
            }
        }

        public DateTime StopTime
        {
            get
            {
                return _streamStopTime;
            }
        }

        public TimeSpan Duration
        {
            get
            {
                if (_isStreaming)
                    return DateTime.Now - _streamStartTime;
                else
                    return _streamStopTime - _streamStartTime;
            }
        }

        public event OnStreamStatusChanged StreamStatusChanged
        {
            add
            {
                lock (this)
                {
                    _streamStatusChanged += value;
                }
            }

            remove
            {
                lock (this)
                {
                    _streamStatusChanged -= value;
                }
            }
        }

        public override string Description
        {
            get
            {
                return "Interacts with Open Broadcaster Software to reliably determine stream start/stop times.";
            }
        }

        public override string Name
        {
            get
            {
                return "OBS Stream Status";
            }
        }

        public override bool HasPreferences
        {
            get
            {
                return false;
            }
        }

        public override bool LoadService()
        {
            _isConnected = false;

            _pipeServer = new NamedPipeServerStream(_pipeName, PipeDirection.In, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
            _pipeServer.BeginWaitForConnection(AwaitConnection, _pipeServer);

            return true;
        }

        public override void UnloadService()
        {
            _pipeServer.Close();
            _pipeServer.Dispose();
        }

        private void AwaitConnection(IAsyncResult ar)
        {
            try
            {
                _pipeServer.EndWaitForConnection(ar);
            }
            catch
            {
                return;
            }

            _isConnected = true;
            byte[] buffer = new byte[1];

            int nRead = _pipeServer.Read(buffer, 0, 1);

            if (nRead > 0)
            {
                bool newValue = buffer[0] == 1;
                if (newValue)
                {
                    _streamStartTime = DateTime.Now;
                }
                else
                {
                    _streamStopTime = DateTime.Now;
                }

                if (newValue != _isStreaming)
                    if (_streamStatusChanged != null)
                        _streamStatusChanged.Invoke(newValue);

                _isStreaming = newValue;
                _pipeServer.Close();
                _pipeServer.Dispose();
                _pipeServer = new NamedPipeServerStream(_pipeName, PipeDirection.In, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
            }

            _pipeServer.BeginWaitForConnection(AwaitConnection, _pipeServer);
        }

        public override void ShowPreferences()
        {
            throw new NotImplementedException();
        }
    }
}
