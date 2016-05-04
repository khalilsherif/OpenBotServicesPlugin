using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBotServicesPlugin.Services.Models
{
    public class VideoRequest
    {
        private string _requestedBy;
        private VideoInformation _videoInformation;
        public string RequestedBy { get { return _requestedBy; } }
        public VideoInformation VideoInformation { get { return _videoInformation; } }
        public VideoRequest(string requestedBy, VideoInformation videoInformation)
        {
            _requestedBy = requestedBy;
            _videoInformation = videoInformation;
        }
    }
}
