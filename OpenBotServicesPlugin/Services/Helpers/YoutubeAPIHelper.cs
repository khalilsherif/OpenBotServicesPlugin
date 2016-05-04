using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using OpenBotServicesPlugin.Services.Models;
using System.Web.Script.Serialization;

namespace OpenBotServicesPlugin.Services.Helpers
{
    internal class YoutubeAPIHelper
    {
        private const string RESOLVER_ENDPOINT = "http://openbot.pragmaticdevelopment.net/video.php";
        private const string ID_PARAMETER_FORMAT = "?video_id={0}";
        private const string SEARCH_PARAMETER_FORMAT = "?search={0}";

        internal static bool GetVideoInformation(string videoId, out VideoInformation info)
        {
            info = VideoInformation.FromJSON(new WebClient().DownloadString(RESOLVER_ENDPOINT + string.Format(ID_PARAMETER_FORMAT, WebUtility.UrlEncode(videoId))));

            return (info != null);
        }

        internal static bool FirstSearchResult(string query, out VideoInformation info)
        {
            info = null;

            string JSONResponse = new WebClient().DownloadString(RESOLVER_ENDPOINT + string.Format(SEARCH_PARAMETER_FORMAT, WebUtility.UrlEncode(query)));

            JavaScriptSerializer deserializer = new JavaScriptSerializer();
            var deserializedData = deserializer.Deserialize<IDictionary<string, object>>(JSONResponse);

            if (!deserializedData.ContainsKey("pageInfo"))
                return false;

            var pageInfo = deserializedData["pageInfo"] as IDictionary<string, object>;

            if (!pageInfo.ContainsKey("totalResults") || (int)pageInfo["totalResults"] < 1)
                return false;

            if (!deserializedData.ContainsKey("items"))
                return false;

            var items = deserializedData["items"] as object[];

            var item = items[0] as IDictionary<string, object>;

            if (!item.ContainsKey("id"))
                return false;

            var idHolder = item["id"] as IDictionary<string, object>;

            if (!idHolder.ContainsKey("videoId"))
                return false;

            string videoId = (string)idHolder["videoId"];

            return GetVideoInformation(videoId, out info);
        }
    }
}
