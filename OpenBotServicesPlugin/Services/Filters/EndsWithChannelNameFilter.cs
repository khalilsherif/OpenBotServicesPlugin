using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBotServicesPlugin.Services.Models;

namespace OpenBotServicesPlugin.Services.Filters
{
    /// <summary>
    /// Filters channels that end with a specified string.
    /// Credit to Masatoshi Windle
    /// https://github.com/m-windle
    /// </summary>
    class EndsWithChannelNameFilter : AbstractFilter
    {        
        public string ChannelName { get; set; }
        public bool CaseInsensitive { get; set; }

        public EndsWithChannelNameFilter()
        {
            ChannelName = "";
            CaseInsensitive = false;
        }
        public override bool Filter(VideoInformation videoInformation)
        {
            if (CaseInsensitive)
            {
                return videoInformation.ChannelName.EndsWith(ChannelName, StringComparison.CurrentCultureIgnoreCase);
            }

            return videoInformation.ChannelName.EndsWith(ChannelName);
        }
    }
}
