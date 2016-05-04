using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBotServicesPlugin.Services.Models;

namespace OpenBotServicesPlugin.Services.Filters
{
    /// <summary>
    /// Filters channels that exactly match a specified string.
    /// Credit to Masatoshi Windle
    /// https://github.com/m-windle
    /// </summary>
    class ExactChannelNameFilter : AbstractFilter
    {        
        public string ChannelName { get; set; }
        public bool CaseInsensitive { get; set; }
        public ExactChannelNameFilter()
        {
            ChannelName = "";
            CaseInsensitive = false;
        }
        public override bool Filter(VideoInformation videoInformation)
        {
            if (CaseInsensitive)
            {
                return videoInformation.ChannelName.ToLower().Equals(ChannelName.ToLower());
            }

            return videoInformation.ChannelName.Equals(ChannelName);
        }
    }
}
