using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBotServicesPlugin.Services.Models;

namespace OpenBotServicesPlugin.Services.Interfaces
{
    public interface IVideoFilter
    {
        bool Filter(VideoInformation videoInformation);
    }
}
