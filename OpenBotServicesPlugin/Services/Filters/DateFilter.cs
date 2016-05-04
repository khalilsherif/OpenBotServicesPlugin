using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBotServicesPlugin.Services.Models;

namespace OpenBotServicesPlugin.Services.Filters
{
    /// <summary>
    /// Filters videos that were uploaded between specified dates inclusively
    /// Credit to Masatoshi Windle
    /// https://github.com/m-windle
    /// </summary>
    class DateFilter : AbstractFilter
    {
        public DateTime MaximumDate { get; set; }
        public DateTime MinimumDate { get; set; }

        public DateFilter()
        {
            MinimumDate = DateTime.MinValue;
            MaximumDate = DateTime.MaxValue;
        }
        public override bool Filter(VideoInformation videoInformation)
        {
            return (videoInformation.DatePublished >= MinimumDate) && (videoInformation.DatePublished <= MaximumDate);
        }
    }
}
