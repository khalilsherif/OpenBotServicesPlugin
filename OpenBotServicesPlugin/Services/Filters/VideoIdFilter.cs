using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBotServicesPlugin.Services.Models;

namespace OpenBotServicesPlugin.Services.Filters
{
    /// <summary>
    /// Filters videos with a specified VideoId
    /// Credit to Masatoshi Windle
    /// https://github.com/m-windle
    /// </summary>
    class VideoIdFilter : AbstractFilter
    {
        public string VideoId { get; set; }

        public VideoIdFilter()
        {
            VideoId = "";
        }

        public override bool Filter(VideoInformation videoInformation)
        {
            return videoInformation.VideoID.ToLower().Equals(VideoId.ToLower());
        }
    }
}
