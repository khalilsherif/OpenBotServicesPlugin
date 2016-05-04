using System;
using System.Data;
using System.Data.Common;
using OpenBot.Plugins;
using OpenBot.Plugins.Interfaces;
using OpenBotServicesPlugin.Handlers;
using OpenBotServicesPlugin.Services;
using OpenBotServicesPlugin.Handlers.Factories;

namespace OpenBotServicesPlugin
{
    public class OpenBotServicesPlugin : AbstractPlugin
    {
        public override IChatHandlerFactory[] ChatHandlerFactories
        {
            get
            {
                return new IChatHandlerFactory[]
                {
                    new DefaultChatHandlerFactory<BasicChatHandler>("Basic Chat Responses"),
                    new SongRequestFactory(),
                };
            }
        }

        public override string Description
        {
            get
            {
                return "Provides basic services for OpenBot plugins to implement.";
            }
        }

        public override string Name
        {
            get
            {
                return "OpenBot Services";
            }
        }

        public override IServiceFactory[] ServiceFactories
        {
            get
            {
                return new IServiceFactory[]
                {
                    new DefaultServiceFactory<OBSStreamStatusService>(),
                    new DefaultServiceFactory<OpenBotCurrencyService>(),
                    new DefaultServiceFactory<ScheduledEventService>(),
                    new DefaultServiceFactory<SongRequestService>()
                };
            }
        }

        public override bool LoadPlugin()
        {
            return true;
        }

        public override void UnloadPlugin()
        {
            //Free any resources
        }
    }
}
