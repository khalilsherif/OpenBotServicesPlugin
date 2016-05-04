using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBotServicesPlugin.Services.Models;

namespace OpenBotServicesPlugin.Services.Filters
{
    /// <summary>
    /// Filters videos with a minimum and maximum number of views inclusively
    /// Credit to Masatoshi Windle
    /// https://github.com/m-windle
    /// </summary>
    public class ViewCountFilter : AbstractFilter
    {
        public long MinimumViews { get; set; }
        public long MaximumViews { get; set; }

        public ViewCountFilter()
        {
            MinimumViews = 0;
            MaximumViews = long.MaxValue;
        }
        public override bool Filter(VideoInformation videoInformation)
        {
            return (videoInformation.ViewCount >= MinimumViews) && (videoInformation.ViewCount <= MaximumViews);
        }
    }
}
