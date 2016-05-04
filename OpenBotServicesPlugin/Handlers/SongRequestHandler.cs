using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using OpenBot.Plugins;
using OpenBot.Plugins.Interfaces;
using OpenBot.Plugins.Delegates;
using OpenBot.Plugins.Matches;
using OpenBotServicesPlugin.Services;
using OpenBotServicesPlugin.Services.Models;
using OpenBotServicesPlugin.Services.Filters;

namespace OpenBotServicesPlugin.Handlers
{
    public class SongRequestHandler : AbstractChatHandler
    {
        public override Dictionary<IMessageMatch, MessageReceivedDelegate> MessageHandlers
        {
            get
            {
                var retDict = new Dictionary<IMessageMatch, MessageReceivedDelegate>();

                retDict.Add(new AbsoluteMessageMatch("!sr"), OnSongRequestNoParam);
                retDict.Add(new RegexMessageMatch("^!sr .{1}.*$"), OnSongRequestAttempt);
                retDict.Add(new AbsoluteMessageMatch("!currentsong"), OnSongRequestCurrent);
                retDict.Add(new AbsoluteMessageMatch("!sropen"), OnSongRequestOpen);
                return retDict;
            }
        }

        private bool OnSongRequestOpen(IChatUser sender, string message, string[] args, string raw, bool handled)
        {
            SongRequestService srService = API.GetService<SongRequestService>();

            if (srService == null)
            {
                API.Adapter.SendMessage("Song request is not currently enabled.");
                return false;
            }
            
            if(sender.Streamer)
                Process.Start(srService.URL);

            return true;
        }

        private bool OnSongRequestCurrent(IChatUser sender, string message, string[] args, string raw, bool handled)
        {
            SongRequestService srService = API.GetService<SongRequestService>();

            if (srService == null)
            {
                API.Adapter.SendMessage("Song request is not currently enabled.");
                return false;
            }

            if (srService.CurrentSong == null)
            {
                API.Adapter.SendMessage("No song is currently playing.");
            }
            else
            {
                API.Adapter.SendMessage("Now playing " + srService.CurrentSong.VideoInformation.Title);
            }

            return true;
        }

        private bool OnSongRequestAttempt(IChatUser sender, string message, string[] args, string raw, bool handled)
        {
            SongRequestService srService = API.GetService<SongRequestService>();

            if (srService == null)
            {
                API.Adapter.SendMessage("Song request is not currently enabled.");
                return false;
            }


            string query = message.Remove(0, message.IndexOf(' ') + 1);

            VideoInformation info;
            if (srService.GetVideoInformation(query, out info))
            {
                if(srService.IsValidFiltered(info))
                {
                    srService.AddToPrimaryPlaylist(info, sender.Username);
                    API.Adapter.SendMessage(string.Format("{0} has been added to the playlist.", info.Title));
                }
                else
                {
                    API.Adapter.SendMessage("The requested video does not meet requirements.");
                }
            }
            else
            {
                API.Adapter.SendMessage("Invalid request.");
            }

            return true;
        }

        private bool OnSongRequestNoParam(IChatUser sender, string message, string[] args, string raw, bool handled)
        {
            API.Adapter.SendMessage("To request a song, type !sr [youtube url or video id]");
            return true;
        }

        public override Dictionary<IMessageMatch, RawCommandReceivedDelegate> RawCommandHandlers
        {
            get
            {
                return null;
            }
        }

        public override void Created()
        {

        }

        public override void Destroyed()
        {
            
        }

        public override void ShowPreferences()
        {
            throw new NotImplementedException();
        }
    }
}
