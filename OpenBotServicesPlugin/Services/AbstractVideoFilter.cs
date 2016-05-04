using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBotServicesPlugin.Services.Interfaces;
using OpenBotServicesPlugin.Services.Models;

namespace OpenBotServicesPlugin.Services
{
    public abstract class AbstractVideoFilter : MarshalByRefObject, IVideoFilter 
    {
        public abstract bool Filter(VideoInformation videoInformation);
    }
}
