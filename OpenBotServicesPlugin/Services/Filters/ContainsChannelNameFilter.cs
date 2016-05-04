using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBotServicesPlugin.Services.Models;

namespace OpenBotServicesPlugin.Services.Filters
{
    /// <summary>
    /// Filters channels that contain the specified string.
    /// Credit to Masatoshi Windle
    /// https://github.com/m-windle
    /// </summary>
    class ContainsChannelNameFilter : AbstractFilter
    {
        public string ChannelName { get; set; }
        public bool CaseInsensitive { get; set; }
        public ContainsChannelNameFilter()
        {
            ChannelName = "";
            CaseInsensitive = false;
        }
        public override bool Filter(VideoInformation videoInformation)
        {
            if (CaseInsensitive)
            {
                return videoInformation.ChannelName.ToLower().Contains(ChannelName.ToLower());
            }

            return videoInformation.ChannelName.Contains(ChannelName);
        }
    }
}
