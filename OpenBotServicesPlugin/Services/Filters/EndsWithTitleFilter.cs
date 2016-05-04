using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBotServicesPlugin.Services.Models;

namespace OpenBotServicesPlugin.Services.Filters
{
    /// <summary>
    /// Filters titles that end with a specified string.
    /// Credit to Masatoshi Windle
    /// https://github.com/m-windle
    /// </summary>
    class EndsWithTitleFilter : AbstractFilter
    {
        public string Title { get; set; }
        public bool CaseInsensitive { get; set; }

        public EndsWithTitleFilter()
        {
            Title = "";
            CaseInsensitive = false;
        }

        public override bool Filter(VideoInformation videoInformation)
        {
            if (CaseInsensitive)
            {
                return videoInformation.Title.EndsWith(Title, StringComparison.CurrentCultureIgnoreCase);
            }

            return videoInformation.Title.EndsWith(Title); 
        }
    }
}
