using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBotServicesPlugin.Services.Models;

namespace OpenBotServicesPlugin.Services.Filters
{
    /// <summary>
    /// Filters videos with a specified duration
    /// Credit to Masatoshi Windle
    /// https://github.com/m-windle
    /// </summary>
    class DurationFilter : AbstractFilter
    {
        public TimeSpan MinimumLength { get; set; }
        public TimeSpan MaximumLength { get; set; }

        public DurationFilter()
        {
            MinimumLength = TimeSpan.MinValue;
            MaximumLength = TimeSpan.MaxValue;
        }
        public override bool Filter(VideoInformation videoInformation)
        {
            return (videoInformation.Duration >= MinimumLength) && (videoInformation.Duration <= MaximumLength);
        }
    }
}
