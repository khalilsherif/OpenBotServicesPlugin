using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBotServicesPlugin.Services.Models;

namespace OpenBotServicesPlugin.Services.Filters
{
    /// <summary>
    /// Filters categories that match a specified integer
    /// Credit to Masatoshi Windle
    /// https://github.com/m-windle
    /// </summary>
    public class CategoryFilter : AbstractFilter
    {
        public int Category { get; set; }

        public CategoryFilter()
        {
            Category = 10;
        }
        public override bool Filter(VideoInformation videoInformation)
        {
            return (videoInformation.Category == Category);
        }
    }
}
