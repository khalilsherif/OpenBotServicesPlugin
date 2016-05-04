using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBotServicesPlugin.Services.Models;

namespace OpenBotServicesPlugin.Services.Filters
{
    /// <summary>
    /// Filters videos with a license
    /// Credit to Masatoshi Windle
    /// https://github.com/m-windle
    /// </summary>
    class LicenseFilter : AbstractFilter
    {
        public bool Licensed { get; set; }
        public LicenseFilter()
        {
            Licensed = false;
        }
        public override bool Filter(VideoInformation videoInformation)
        {
            return (videoInformation.LicensedContent == Licensed);
        }
    }
}
