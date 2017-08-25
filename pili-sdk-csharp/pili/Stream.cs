using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pili_sdk_csharp.pili_qiniu;

namespace pili_sdk_csharp.pili
{
    public class Stream
    {
        public const string ORIGIN = "ORIGIN";
        private readonly string id;
        private readonly Credentials mCredentials;
        private readonly string mStreamJsonStr;
        private readonly string[] profiles;


        public Stream(JObject jsonObj)
        {
            //  System.out.println("Stream:" + jsonObj.toString());
            id = jsonObj["id"].ToString();
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
                profiles = JsonConvert.DeserializeAnonymousType(jsonObj["profiles"].ToString(), profiles);
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

            mStreamJsonStr = jsonObj.ToString();
        }


        public Stream(JObject jsonObject, Credentials credentials)
            : this(jsonObject)
        {
            mCredentials = credentials;
        }

        public string PlaybackHttpHost { get; }

        public string PlayRtmpHost { get; }

        public string LiveHdlHost { get; }

        public string PlayHttpHost { get; }

        public string LiveHttpHost { get; }


        public string LiveHlsHost { get; }

        public virtual string[] Profiles => profiles;

        public virtual string PublishRtmpHost { get; }

        public virtual string LiveRtmpHost { get; }

        public virtual string StreamId => id;

        public virtual string HubName { get; }

        public virtual string CreatedAt { get; }

        public virtual string UpdatedAt { get; }

        public virtual string Title { get; }

        public virtual string PublishKey { get; }

        public virtual string PublishSecurity { get; }

        public virtual bool Disabled { get; }


        public virtual Stream update(string publishKey, string publishSecrity, bool disabled)
        {
            return API.updateStream(mCredentials, id, publishKey, publishSecrity, disabled);
        }


        public virtual SegmentList segments()
        {
            return API.getStreamSegments(mCredentials, id, 0, 0, 0);
        }


        public virtual SegmentList segments(long start, long end)
        {
            return API.getStreamSegments(mCredentials, id, start, end, 0);
        }


        public virtual SegmentList segments(long start, long end, int limit)
        {
            return API.getStreamSegments(mCredentials, id, start, end, limit);
        }


        public virtual Status status()
        {
            return API.getStreamStatus(mCredentials, id);
        }


        public virtual string rtmpPublishUrl()
        {
            return API.publishUrl(this, 0);
        }

        public virtual IDictionary<string, string> rtmpLiveUrls()
        {
            return API.rtmpLiveUrl(this);
        }

        public virtual IDictionary<string, string> hlsLiveUrls()
        {
            return API.hlsLiveUrl(this);
        }

        public virtual IDictionary<string, string> hlsPlaybackUrls(long start, long end)
        {
            return API.hlsPlaybackUrl(mCredentials, id, start, end);
        }

        public virtual IDictionary<string, string> httpFlvLiveUrls()
        {
            return API.httpFlvLiveUrl(this);
        }


        public virtual string delete()
        {
            return API.deleteStream(mCredentials, id);
        }

        public virtual string toJsonString()
        {
            return mStreamJsonStr;
        }


        public virtual SaveAsResponse saveAs(string fileName, string format, long startTime, long endTime, string notifyUrl, string pipleline)
        {
            return API.saveAs(mCredentials, id, fileName, format, startTime, endTime, notifyUrl, pipleline);
        }

        public virtual SaveAsResponse saveAs(string fileName, string format, long startTime, long endTime)
        {
            return saveAs(fileName, format, startTime, endTime, null, null);
        }

        public virtual SaveAsResponse saveAs(string fileName, string format, string notifyUrl, string pipleline)
        {
            return saveAs(fileName, format, 0, 0, notifyUrl, pipleline);
        }

        public virtual SaveAsResponse saveAs(string fileName, string format)
        {
            return saveAs(fileName, format, 0, 0, null, null);
        }

        public virtual SnapshotResponse snapshot(string name, string format)
        {
            return API.snapshot(mCredentials, id, name, format, 0, null);
        }

        public virtual SnapshotResponse snapshot(string name, string format, string notifyUrl)
        {
            return API.snapshot(mCredentials, id, name, format, 0, notifyUrl);
        }

        public virtual SnapshotResponse snapshot(string name, string format, long time, string notifyUrl)
        {
            return API.snapshot(mCredentials, id, name, format, time, notifyUrl);
        }


        public virtual Stream enable()
        {
            return API.updateStream(mCredentials, id, null, null, false);
        }

        public virtual Stream disable()
        {
            return API.updateStream(mCredentials, id, null, null, true);
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
            private readonly string mJsonString;

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
                mJsonString = jsonObj.ToString();
            }

            public virtual string Url { get; }

            public virtual string TargetUrl { get; }

            public virtual string PersistentId { get; }

            public override string ToString()
            {
                return mJsonString;
            }
        }

        public class SnapshotResponse
        {
            private readonly string mJsonString;

            public SnapshotResponse(JObject jsonObj)
            {
                TargetUrl = jsonObj["targetUrl"].ToString();
                PersistentId = jsonObj.GetValue("persistentId") == null ? null : jsonObj["persistentId"].ToString();
                mJsonString = jsonObj.ToString();
            }

            public virtual string TargetUrl { get; }

            public virtual string PersistentId { get; }

            public override string ToString()
            {
                return mJsonString;
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
            private readonly IList<Segment> segmentList;


            public SegmentList(JObject jsonObj)
            {
                segmentList = new List<Segment>();
                var jlist = JArray.Parse(jsonObj["segments"].ToString());
                for (var i = 0; i < jlist.Count; ++i)
                {
                    var tempo = JObject.Parse(jlist[i].ToString());
                    segmentList.Add(new Segment((long)tempo["start"], (long)tempo["end"]));
                }
            }

            public virtual IList<Segment> getSegmentList()
            {
                return segmentList;
            }
        }

        public class Status
        {
            private readonly string mJsonString;
            private readonly string status;

            public Status(JObject jsonObj)
            {
                Addr = jsonObj["addr"].ToString();
                status = jsonObj["status"].ToString();
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
                mJsonString = jsonObj.ToString();
            }

            public virtual string Addr { get; }

            public virtual string StartFrom { get; }

            public virtual float BytesPerSecond { get; }

            public virtual FramesPerSecond FramesPerSecond { get; }

            public virtual string getStatus()
            {
                return status;
            }

            public override string ToString()
            {
                return mJsonString;
            }
        }

        public class StreamList
        {
            private readonly IList<Stream> itemList;
            private readonly string marker;

            public StreamList(JObject jsonObj, Credentials auth)
            {
                marker = jsonObj["marker"].ToString();
                Console.WriteLine("this.marker-----" + marker);

                try
                {
                    var record = jsonObj["items"];
                    itemList = new List<Stream>();
                    foreach (JObject jp in record)
                    {
                        itemList.Add(new Stream(JObject.Parse(jp.ToString()), auth));
                    }
                }
                catch (InvalidCastException e)
                {
                    Console.WriteLine(e.ToString());
                    Console.Write(e.StackTrace);
                }
            }

            public virtual string Marker => marker;

            public virtual IList<Stream> Streams => itemList;
        }
    }
}
