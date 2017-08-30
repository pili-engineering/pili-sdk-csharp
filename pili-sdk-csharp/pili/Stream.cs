using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pili_sdk_csharp.pili_qiniu;

namespace pili_sdk_csharp.pili
{
    public class Stream
    {
        public const string Origin = "ORIGIN";
        private readonly string _id;
        private readonly Credentials _mCredentials;
        private readonly string _mStreamJsonStr;
        private readonly string[] _profiles;


        public Stream(JObject jsonObj)
        {
            //  System.out.println("Stream:" + jsonObj.toString());
            _id = jsonObj["id"].ToString();
            HubName = jsonObj["hub"].ToString();
            CreatedAt = jsonObj["createdAt"].ToString();
            UpdatedAt = jsonObj["updatedAt"].ToString();
            Title = jsonObj["title"].ToString();
            PublishKey = jsonObj["publishKey"].ToString();
            PublishSecurity = jsonObj["publishSecurity"].ToString();
            Disabled = (bool)jsonObj["disabled"];

            if (jsonObj["profiles"] != null)
            {
                Console.WriteLine("profiles--------" + jsonObj["profiles"]);
                _profiles = JsonConvert.DeserializeAnonymousType(jsonObj["profiles"].ToString(), _profiles);
            }

            if (jsonObj["hosts"]["publish"] != null)
            {
                PublishRtmpHost = jsonObj["hosts"]["publish"]["rtmp"].ToString();
            }
            if (jsonObj["hosts"]["live"] != null)
            {
                LiveRtmpHost = jsonObj["hosts"]["live"]["rtmp"].ToString();
                LiveHdlHost = jsonObj["hosts"]["live"]["hdl"].ToString();
                LiveHlsHost = jsonObj["hosts"]["live"]["hls"].ToString();
                LiveHttpHost = jsonObj["hosts"]["live"]["hls"].ToString();
            }
            if (jsonObj["hosts"]["playback"] != null)
            {
                PlaybackHttpHost = jsonObj["hosts"]["playback"]["hls"].ToString();
            }
            if (jsonObj["hosts"]["play"] != null)
            {
                PlayHttpHost = jsonObj["hosts"]["play"]["http"].ToString();
                PlayRtmpHost = jsonObj["hosts"]["play"]["rtmp"].ToString();
            }

            _mStreamJsonStr = jsonObj.ToString();
        }


        public Stream(JObject jsonObject, Credentials credentials)
            : this(jsonObject)
        {
            _mCredentials = credentials;
        }

        public string PlaybackHttpHost { get; }

        public string PlayRtmpHost { get; }

        public string LiveHdlHost { get; }

        public string PlayHttpHost { get; }

        public string LiveHttpHost { get; }


        public string LiveHlsHost { get; }

        public virtual string[] Profiles => _profiles;

        public virtual string PublishRtmpHost { get; }

        public virtual string LiveRtmpHost { get; }

        public virtual string StreamId => _id;

        public virtual string HubName { get; }

        public virtual string CreatedAt { get; }

        public virtual string UpdatedAt { get; }

        public virtual string Title { get; }

        public virtual string PublishKey { get; }

        public virtual string PublishSecurity { get; }

        public virtual bool Disabled { get; }


        public virtual Stream Update(string publishKey, string publishSecrity, bool disabled)
        {
            return API.UpdateStream(_mCredentials, _id, publishKey, publishSecrity, disabled);
        }


        public virtual SegmentList Segments()
        {
            return API.GetStreamSegments(_mCredentials, _id, 0, 0, 0);
        }


        public virtual SegmentList Segments(long start, long end)
        {
            return API.GetStreamSegments(_mCredentials, _id, start, end, 0);
        }


        public virtual SegmentList Segments(long start, long end, int limit)
        {
            return API.GetStreamSegments(_mCredentials, _id, start, end, limit);
        }


        public virtual StreamStatus Status()
        {
            return API.GetStreamStatus(_mCredentials, _id);
        }


        public virtual string RtmpPublishUrl()
        {
            return API.PublishUrl(this, 0);
        }

        public virtual IDictionary<string, string> RtmpLiveUrls()
        {
            return API.RtmpLiveUrl(this);
        }

        public virtual IDictionary<string, string> HlsLiveUrls()
        {
            return API.HlsLiveUrl(this);
        }

        public virtual IDictionary<string, string> HlsPlaybackUrls(long start, long end)
        {
            return API.HlsPlaybackUrl(_mCredentials, _id, start, end);
        }

        public virtual IDictionary<string, string> HttpFlvLiveUrls()
        {
            return API.HttpFlvLiveUrl(this);
        }


        public virtual string Delete()
        {
            return API.DeleteStream(_mCredentials, _id);
        }

        public virtual string ToJsonString()
        {
            return _mStreamJsonStr;
        }


        public virtual SaveAsResponse SaveAs(string fileName, string format, long startTime, long endTime, string notifyUrl, string pipleline)
        {
            return API.SaveAs(_mCredentials, _id, fileName, format, startTime, endTime, notifyUrl, pipleline);
        }

        public virtual SaveAsResponse SaveAs(string fileName, string format, long startTime, long endTime)
        {
            return SaveAs(fileName, format, startTime, endTime, null, null);
        }

        public virtual SaveAsResponse SaveAs(string fileName, string format, string notifyUrl, string pipleline)
        {
            return SaveAs(fileName, format, 0, 0, notifyUrl, pipleline);
        }

        public virtual SaveAsResponse SaveAs(string fileName, string format)
        {
            return SaveAs(fileName, format, 0, 0, null, null);
        }

        public virtual SnapshotResponse Snapshot(string name, string format)
        {
            return API.Snapshot(_mCredentials, _id, name, format, 0, null);
        }

        public virtual SnapshotResponse Snapshot(string name, string format, string notifyUrl)
        {
            return API.Snapshot(_mCredentials, _id, name, format, 0, notifyUrl);
        }

        public virtual SnapshotResponse Snapshot(string name, string format, long time, string notifyUrl)
        {
            return API.Snapshot(_mCredentials, _id, name, format, time, notifyUrl);
        }


        public virtual Stream Enable()
        {
            return API.UpdateStream(_mCredentials, _id, null, null, false);
        }

        public virtual Stream Disable()
        {
            return API.UpdateStream(_mCredentials, _id, null, null, true);
        }

        public class Segment
        {
            public Segment(long start, long end)
            {
                Start = start;
                End = end;
            }

            public virtual long Start { get; }

            public virtual long End { get; }
        }


        public class SaveAsResponse
        {
            private readonly string _mJsonString;

            public SaveAsResponse(JObject jsonObj)
            {
                Url = jsonObj["url"].ToString();
                try
                {
                    TargetUrl = jsonObj["targetUrl"].ToString();
                    //
                }
                catch (NullReferenceException)
                {
                    // do nothing. ignore.
                }
                PersistentId = jsonObj["persistentId"].ToString();
                _mJsonString = jsonObj.ToString();
            }

            public virtual string Url { get; }

            public virtual string TargetUrl { get; }

            public virtual string PersistentId { get; }

            public override string ToString()
            {
                return _mJsonString;
            }
        }

        public class SnapshotResponse
        {
            private readonly string _mJsonString;

            public SnapshotResponse(JObject jsonObj)
            {
                TargetUrl = jsonObj["targetUrl"].ToString();
                PersistentId = jsonObj.GetValue("persistentId") == null ? null : jsonObj["persistentId"].ToString();
                _mJsonString = jsonObj.ToString();
            }

            public virtual string TargetUrl { get; }

            public virtual string PersistentId { get; }

            public override string ToString()
            {
                return _mJsonString;
            }
        }

        public class FramesPerSecond
        {
            public FramesPerSecond(float audio, float video, float data)
            {
                Audio = audio;
                Video = video;
                Data = data;
            }

            public virtual float Audio { get; }

            public virtual float Video { get; }

            public virtual float Data { get; }
        }

        public class SegmentList
        {
            private readonly IList<Segment> _segmentList;


            public SegmentList(JObject jsonObj)
            {
                _segmentList = new List<Segment>();
                var jlist = JArray.Parse(jsonObj["segments"].ToString());
                for (var i = 0; i < jlist.Count; ++i)
                {
                    var tempo = JObject.Parse(jlist[i].ToString());
                    _segmentList.Add(new Segment((long)tempo["start"], (long)tempo["end"]));
                }
            }

            public virtual IList<Segment> GetSegmentList()
            {
                return _segmentList;
            }
        }

        public class StreamStatus
        {
            private readonly string _mJsonString;
            private readonly string _status;

            public StreamStatus(JObject jsonObj)
            {
                Addr = jsonObj["addr"].ToString();
                _status = jsonObj["status"].ToString();
                var startFrominit = (DateTime)jsonObj["startFrom"];
                StartFrom = startFrominit.ToString("yyyy-MM-ddTHH:mm:ssZ");
                try
                {
                    BytesPerSecond = (float)jsonObj["bytesPerSecond"];
                    var audio = (float)jsonObj["framesPerSecond"]["audio"];
                    var video = (float)jsonObj["framesPerSecond"]["video"];
                    var data = (float)jsonObj["framesPerSecond"]["data"];
                    FramesPerSecond = new FramesPerSecond(audio, video, data);
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e.ToString());
                    Console.Write(e.StackTrace);
                }
                _mJsonString = jsonObj.ToString();
            }

            public virtual string Addr { get; }

            public virtual string StartFrom { get; }

            public virtual float BytesPerSecond { get; }

            public virtual FramesPerSecond FramesPerSecond { get; }

            public virtual string GetStatus()
            {
                return _status;
            }

            public override string ToString()
            {
                return _mJsonString;
            }
        }

        public class StreamList
        {
            private readonly IList<Stream> _itemList;
            private readonly string _marker;

            public StreamList(JObject jsonObj, Credentials auth)
            {
                _marker = jsonObj["marker"].ToString();
                Console.WriteLine("this.marker-----" + _marker);

                try
                {
                    var record = jsonObj["items"];
                    _itemList = new List<Stream>();
                    foreach (var jp in record)
                    {
                        _itemList.Add(new Stream(JObject.Parse(jp.ToString()), auth));
                    }
                }
                catch (InvalidCastException e)
                {
                    Console.WriteLine(e.ToString());
                    Console.Write(e.StackTrace);
                }
            }

            public virtual string Marker => _marker;

            public virtual IList<Stream> Streams => _itemList;
        }
    }
}
