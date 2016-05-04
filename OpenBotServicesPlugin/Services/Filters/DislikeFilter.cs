using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBotServicesPlugin.Services.Models;

namespace OpenBotServicesPlugin.Services.Filters
{
    /// <summary>
    /// Filters videos that have a maximum and minimum number of dislikes inclusively
    /// Credit to Masatoshi Windle
    /// https://github.com/m-windle
    /// </summary>
    public class DislikeFilter : AbstractFilter
    {
        public long MinDislikes { get; set; }
        public long MaxDislikes { get; set; }

        public DislikeFilter()
        {
            MinDislikes = 0;
            MaxDislikes = long.MaxValue;
        }
        public override bool Filter(VideoInformation videoInformation)
        {
            return (videoInformation.DislikeCount >= MinDislikes) && (videoInformation.DislikeCount <= MaxDislikes);
        }
    }
}
