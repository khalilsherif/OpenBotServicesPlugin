using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBotServicesPlugin.Services.Models;
using OpenBotServicesPlugin.Services.Interfaces;

namespace OpenBotServicesPlugin.Services.Filters
{
    public abstract class AbstractFilter : MarshalByRefObject, IVideoFilter
    {
        public abstract bool Filter(VideoInformation videoInformation);
    }
}
