using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBot.Plugins;
using OpenBot.Plugins.Interfaces;

namespace OpenBotServicesPlugin.Handlers.Factories
{
    public class SongRequestFactory : AbstractChatHandlerFactory
    {
        public override string Description
        {
            get
            {
                return "Provides chat functionality for song request";
            }
        }

        public override string Name
        {
            get
            {
                return "Song Requests";
            }
        }

        public override bool SingleInstance
        {
            get
            {
                return true;
            }
        }

        public override IChatHandler Create()
        {
            return new SongRequestHandler();
        }
    }
}
