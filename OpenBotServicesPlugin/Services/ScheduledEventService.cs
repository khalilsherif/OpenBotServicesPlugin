using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBot.Plugins;

namespace OpenBotServicesPlugin.Services
{
    /// <summary>
    /// Allows for delayed event execution.
    /// Credit to Masatoshi Windle
    /// https://github.com/m-windle
    /// </summary>
    public class ScheduledEventService : AbstractService
    {
        public delegate void timedDelegate();

        public override string Name
        {
            get { return "Event Scheduling Service"; }
        }

        public override string Description
        {
            get { return "Provides delayed events to Handlers."; }
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
            return true;
        }

        public override void UnloadService() { }
        
        public async void AddTimedEvent(timedDelegate timedEvent, TimeSpan delay)
        {
            await timedDelay(delay);
            timedEvent();
        }

        async Task timedDelay(TimeSpan delay)
        {
            await Task.Delay((int)delay.TotalMilliseconds);
        }

        public override void ShowPreferences()
        {
            throw new NotImplementedException();
        }
    }
}
