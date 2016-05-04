using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBotServicesPlugin.Services.Models;

namespace OpenBotServicesPlugin.Services.Filters
{
    /// <summary>
    /// Allows for searching for channel names that begin with a specified string
    /// Credit to Masatoshi Windle
    /// https://github.com/m-windle
    /// </summary>
    class BeginsWithChannelNameFilter : AbstractFilter 
    {
        public string ChannelName { get; set; }
        public bool CaseInsensitive { get; set; }

        public BeginsWithChannelNameFilter()
        {
            ChannelName = "";
            CaseInsensitive = false;
        }
        public override bool Filter(VideoInformation videoInformation)
        {
            if (CaseInsensitive)
            {
                return videoInformation.ChannelName.StartsWith(ChannelName, StringComparison.CurrentCultureIgnoreCase);
            }

            return videoInformation.ChannelName.StartsWith(ChannelName);
        }
    }
}
