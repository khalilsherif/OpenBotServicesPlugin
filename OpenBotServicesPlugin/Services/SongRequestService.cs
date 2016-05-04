using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using OpenBot.Plugins;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using OpenBotServicesPlugin.Services.Models;
using OpenBotServicesPlugin.Services.Interfaces;

namespace OpenBotServicesPlugin.Services
{
    public class SongRequestService : AbstractService
    {
        public override string Description
        {
            get
            {
                return "Provides Song Request functionality";
            }
        }

        public override bool HasPreferences
        {
            get
            {
                return true;
            }
        }

        public override string Name
        {
            get
            {
                return "Song Requests";
            }
        }

        public string URL
        {
            get
            {
                return string.Format("http://{0}/", _listeningUrl);
            }
        }

        public VideoRequest[] PrimaryPlaylist
        {
            get
            {
                return _primaryPlaylist.ToArray();
            }
        }

        public VideoInformation[] SecondaryPlaylist
        {
            get
            {
                return _secondaryPlaylist.ToArray();
            }
        }

        public IVideoFilter[] Filters
        {
            get
            {
                return _filters.ToArray();
            }
        }
        
        public VideoRequest CurrentSong
        {
            get
            {
                return _currentSong;
            }
        }

        public bool VideoIdsOnly
        {
            get
            {
                return _videoIdOnly;
            }
        }

        private const string REGEX_MATCH = @"(?:youtube(?:-nocookie)?\.com/(?:[^/]+/.+/|(?:v|e(?:mbed)?)/|.*[?&]v=)|youtu\.be/)(?:([a-zA-Z0-9-_]{11}))|(^[a-zA-Z0-9-_]{11})";

        private HttpListener _listener;
        private string _listeningUrl;
        private Dictionary<string, byte[]> _rawData;
        private Semaphore _frameSemaphore;
        private VideoRequest _currentSong;
        private Queue<VideoRequest> _primaryPlaylist;
        private List<VideoInformation> _secondaryPlaylist;
        private List<IVideoFilter> _filters;
        private int _secondaryIndex;
        private bool _videoIdOnly;

        //TODO: Put html source in file statically
        //TODO: Migrate to different WebSocket implementation, built-in only supports Windows 8 and higher.
        public override bool LoadService()
        {
            Socket bindSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            bindSocket.Bind(new IPEndPoint(IPAddress.Loopback, 0));

            int targetPort = (bindSocket.LocalEndPoint as IPEndPoint).Port;

            bindSocket.Close();
            bindSocket.Dispose();

            _secondaryIndex = 0;
            _frameSemaphore = new Semaphore(0, 1);
            _rawData = new Dictionary<string, byte[]>();

            _primaryPlaylist = new Queue<VideoRequest>();
            _secondaryPlaylist = new List<VideoInformation>();
            _filters = new List<IVideoFilter>();
            _videoIdOnly = false;

            _rawData["/"] = System.IO.File.ReadAllBytes("X:\\htmlsource.html");

            _listener = new HttpListener();
            _listeningUrl = string.Format("localhost:{0}", targetPort);

            _listener.Prefixes.Add(string.Format("http://{0}/", _listeningUrl));

            _listener.Start();

            AcceptHttpContext();

            return _listener.IsListening;
        }

        private async void AcceptHttpContext()
        {
            while(_listener.IsListening)
            {
                HttpListenerContext ctx = await _listener.GetContextAsync();

                if (ctx.Request.IsWebSocketRequest)
                    HandleWebSocketConnection(ctx);
                else
                    HandleContentRequest(ctx);
            }
        }
        
        private async void HandleContentRequest(HttpListenerContext ctx)
        {
            if (!_rawData.ContainsKey(ctx.Request.RawUrl))
                return;

            byte[] buffer = _rawData[ctx.Request.RawUrl];
            ctx.Response.ContentLength64 = buffer.Length;
            await ctx.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        }

        private async void HandleWebSocketConnection(HttpListenerContext ctx)
        {
            WebSocketContext wsCtx = await ctx.AcceptWebSocketAsync(null);
            WebSocket wSocket = wsCtx.WebSocket;

            if(wSocket.State == WebSocketState.Open)
                ThreadPool.QueueUserWorkItem(ReadWebSocketMessage, wSocket);

            while (wSocket.State == WebSocketState.Open && _listener.IsListening)
            {
                _frameSemaphore.WaitOne();

                byte[] data = Encoding.UTF8.GetBytes(_currentSong.VideoInformation.VideoID);
                await wSocket.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Text, true, new CancellationToken());
            }
        }

        private async void ReadWebSocketMessage(object o)
        {
            WebSocket wSocket = o as WebSocket;

            byte[] buffer = new byte[32];

            while(wSocket.State == WebSocketState.Open && _listener.IsListening)
            {
                try
                {
                    WebSocketReceiveResult result = await wSocket.ReceiveAsync(new ArraySegment<byte>(buffer), new CancellationToken());

                    string data = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    if (data == "NEXT")
                        PlayNext();
                }
                catch { }
            }
        }

        public void AddFilter(IVideoFilter filter)
        {
            _filters.Add(filter);
        }

        public void RemoveFilter(IVideoFilter filter)
        {
            if (_filters.Contains(filter))
                _filters.Remove(filter);
        }

        public bool IsValidVideoIdOrURL(string data)
        {
            return Regex.IsMatch(data, REGEX_MATCH);
        }

        public string GetVideoId(string data)
        {
            IEnumerable<Group> groups = Regex.Match(data, REGEX_MATCH).Groups.Cast<Group>();
            return groups.Where(a => a.Value.Length == 11 && IsValidVideoIdOrURL(a.Value)).First().Value;
        }

        public bool GetVideoInformation(string query, out VideoInformation info)
        {
            info = null;

            if(IsValidVideoIdOrURL(query))
            {
                string videoId = GetVideoId(query);
                return Helpers.YoutubeAPIHelper.GetVideoInformation(videoId, out info);
            }
            else
            {
                if(VideoIdsOnly)
                {
                    return false;
                }

                return Helpers.YoutubeAPIHelper.FirstSearchResult(query, out info);
            }
        }

        public bool IsValidFiltered(VideoInformation info)
        {
            foreach (IVideoFilter i in _filters)
                if (!i.Filter(info))
                    return false;
            return true;
        }

        public bool AddToPrimaryPlaylist(VideoInformation info, string requestedBy = "[SYSTEM]")
        {
            foreach (IVideoFilter i in _filters)
                if (!i.Filter(info))
                    return false;

            VideoRequest requestedInfo = new VideoRequest(requestedBy, info);

            _primaryPlaylist.Enqueue(requestedInfo);

            return true;
        }

        public bool AddToSecondaryPlaylist(VideoInformation info)
        {
            _secondaryPlaylist.Add(info);

            return true;
        }

        public void PlayVideo(VideoRequest request)
        {
            _currentSong = request;
            _frameSemaphore.Release();
        }

        public void PlayNext()
        {
            if(_primaryPlaylist.Count > 0)
            {
                PlayVideo(_primaryPlaylist.Dequeue());
            }
            else
            {
                if (_secondaryIndex >= _secondaryPlaylist.Count)
                    _secondaryIndex = 0;

                PlayVideo(new VideoRequest("", _secondaryPlaylist[_secondaryIndex++]));
            }
        }

        public override void UnloadService()
        {
            _listener.Stop();
        }

        public override void ShowPreferences()
        {
            throw new NotImplementedException();
        }
    }
}
