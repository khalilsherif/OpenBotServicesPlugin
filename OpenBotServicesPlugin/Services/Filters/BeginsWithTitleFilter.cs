using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBotServicesPlugin.Services.Models;

namespace OpenBotServicesPlugin.Services.Filters
{
    /// <summary>
    /// Filters titles of videos that begin with a specified string
    /// Credit to Masatoshi Windle
    /// https://github.com/m-windle
    /// </summary>
    class BeginsWithTitleFilter : AbstractFilter
    {        
        public string Title { get; set; }
        public bool CaseInsensitive { get; set; }

        public BeginsWithTitleFilter()
        {
            Title = "";
            CaseInsensitive = false;
        }

        public override bool Filter(VideoInformation videoInformation)
        {
            if (CaseInsensitive)
            {
                return videoInformation.Title.StartsWith(Title, StringComparison.CurrentCultureIgnoreCase);
            }

            return videoInformation.Title.StartsWith(Title); 
        }
    }
}
