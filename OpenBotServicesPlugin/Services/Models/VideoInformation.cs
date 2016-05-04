using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Web.Script.Serialization;

namespace OpenBotServicesPlugin.Services.Models
{
    public class VideoInformation : MarshalByRefObject
    {
        private string _title;
        private string _id;
        private string _description;
        private string _channelTitle;
        private string _channelId;

        private long _viewCount;
        private long _likeCount;
        private long _dislikeCount;
        private long _commentCount;
        private int _categoryId;

        private TimeSpan _duration;
        private DateTime _publishDate;

        private bool _licensed;

        public string Title { get { return _title; } }
        public string VideoID { get { return _id; } }
        public string Description { get { return _description; } }
        public string ChannelName { get { return _channelTitle; } }
        public string ChannelID { get { return _channelId; } }
        public long ViewCount { get { return _viewCount; } }
        public long LikeCount { get { return _likeCount; } }
        public long DislikeCount { get { return _dislikeCount; } }
        public long CommentCount { get { return _commentCount; } }
        public int Category { get { return _categoryId; } }
        public DateTime DatePublished { get { return _publishDate; } }
        public TimeSpan Duration { get { return _duration; } }
        public bool LicensedContent { get { return _licensed; } }

        protected VideoInformation() { }
        public VideoInformation(string id, string title, string description, string channelTitle, string channelId, long viewCount, long likeCount, long dislikeCount, long commentCount, int categoryId, DateTime publishDate, TimeSpan duration, bool licensed)
        {
            _id = id;
            _title = title;
            _description = description;
            _channelTitle = channelTitle;
            _channelId = channelId;
            _viewCount = viewCount;
            _likeCount = likeCount;
            _dislikeCount = dislikeCount;
            _commentCount = commentCount;
            _categoryId = categoryId;
            _publishDate = publishDate;
            _duration = duration;
            _licensed = licensed;
        }

        internal static VideoInformation FromJSON(string JSONResponse)
        {
            JavaScriptSerializer deserializer = new JavaScriptSerializer();
            var deserializedData = deserializer.Deserialize<IDictionary<string, object>>(JSONResponse);

            if (!deserializedData.ContainsKey("pageInfo"))
                return null;

            var pageInfo = deserializedData["pageInfo"] as IDictionary<string, object>;

            if (!pageInfo.ContainsKey("totalResults") || (int)pageInfo["totalResults"] < 1)
                return null;

            if (!deserializedData.ContainsKey("items"))
                return null;

            var items = deserializedData["items"] as object[];

            var item = items[0] as IDictionary<string, object>;

            if (!item.ContainsKey("id") ||
                !item.ContainsKey("snippet") ||
                !item.ContainsKey("statistics") ||
                !item.ContainsKey("contentDetails"))
                return null;

            var snippet = item["snippet"] as IDictionary<string, object>;
            var statistics = item["statistics"] as IDictionary<string, object>;
            var contentDetails = item["contentDetails"] as IDictionary<string, object>;

            if (!snippet.ContainsKey("title") ||
                !snippet.ContainsKey("description") ||
                !snippet.ContainsKey("channelTitle") ||
                !snippet.ContainsKey("channelId") ||
                !snippet.ContainsKey("publishedAt") ||
                !snippet.ContainsKey("categoryId"))
                return null;

            if (!statistics.ContainsKey("viewCount") ||
                !statistics.ContainsKey("likeCount") ||
                !statistics.ContainsKey("dislikeCount") ||
                !statistics.ContainsKey("commentCount"))
                return null;

            if (!contentDetails.ContainsKey("duration") ||
                !contentDetails.ContainsKey("licensedContent"))
                return null;


            return new VideoInformation(
                    (string)item["id"],
                    (string)snippet["title"],
                    (string)snippet["description"],
                    (string)snippet["channelTitle"],
                    (string)snippet["channelId"],
                    long.Parse((string)statistics["viewCount"]),
                    long.Parse((string)statistics["likeCount"]),
                    long.Parse((string)statistics["dislikeCount"]),
                    long.Parse((string)statistics["commentCount"]),
                    int.Parse((string)snippet["categoryId"]),
                    DateTime.Parse((string)snippet["publishedAt"]),
                    XmlConvert.ToTimeSpan((string)contentDetails["duration"]),
                    (bool)contentDetails["licensedContent"]
                );
        }
    }
}
