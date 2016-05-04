using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBotServicesPlugin.Services.Models;

namespace OpenBotServicesPlugin.Services.Filters
{
    /// <summary>
    /// Filters the number of allowable comments in a video. 
    /// Credit to Masatoshi Windle
    /// https://github.com/m-windle
    /// </summary>
    public class CommentFilter : AbstractFilter
    {
        public long MinComments { get; set; }
        public long MaxComments { get; set; }

        public CommentFilter()
        {
            MinComments = 0;
            MaxComments = long.MaxValue;
        }
        public override bool Filter(VideoInformation videoInformation)
        {
            return (videoInformation.CommentCount >= MinComments) && (videoInformation.CommentCount <= MaxComments);
        }
    }
}
