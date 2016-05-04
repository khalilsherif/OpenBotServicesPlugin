using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBotServicesPlugin.Services.Models;

namespace OpenBotServicesPlugin.Services.Filters
{
    /// <summary>
    /// Filters titles of videos that contain a specified string.
    /// Credit to Masatoshi Windle
    /// https://github.com/m-windle
    /// </summary>
    class ContainsTitleFilter : AbstractFilter
    {
        public string Title { get; set; }
        public bool CaseInsensitive { get; set; }

        public ContainsTitleFilter()
        {
            Title = "";
            CaseInsensitive = false;
        }

        public override bool Filter(VideoInformation videoInformation)
        {
            if (CaseInsensitive)
            {
                return videoInformation.Title.ToLower().Contains(Title.ToLower());
            }

            return videoInformation.Title.Contains(Title); 
        }
    }
}
