using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBotServicesPlugin.Services.Models;

namespace OpenBotServicesPlugin.Services.Filters
{
    public class LikeFilter : AbstractFilter
    {
        public long MinimumLikes { get; set; }
        public long MaximumLikes { get; set; }

        public LikeFilter()
        {
            MinimumLikes = 0;
            MaximumLikes = long.MaxValue;
        }
        public override bool Filter(VideoInformation videoInformation)
        {
            return (videoInformation.LikeCount >= MinimumLikes) && (videoInformation.LikeCount <= MaximumLikes);
        }
    }
}
