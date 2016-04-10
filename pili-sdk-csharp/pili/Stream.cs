using System;
using System.Collections.Generic;
using Newtonsoft;
using Credentials = pili_sdk_csharp.pili_qiniu.Credentials;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace pili_sdk_csharp.pili
{


    public class Stream
    {
        public const string ORIGIN = "ORIGIN";
        private string mStreamJsonStr;
        private Credentials mCredentials;
        private string id;
        private string createdAt; // Time ISO 8601
        private string updatedAt; // Time ISO 8601
        private string title; // Length[4-200]
        private string hub;
        private string publishKey;
        private string publishSecurity; // "static" or "dynamic"
        private bool disabled;
        private string[] profiles = null;
        private string publishRtmpHost;
        private string liveRtmpHost;
        private string playbackHttpHost;
        private string liveHlsHost;
        private string liveHdlHost;
        private string liveHttpHost;
        private string playHttpHost;
        private string playRtmpHost;






        public Stream(JObject jsonObj)
        {
            //  System.out.println("Stream:" + jsonObj.toString());
            id = jsonObj["id"].ToString();
            hub = jsonObj["hub"].ToString();
            createdAt = jsonObj["createdAt"].ToString();
            updatedAt = jsonObj["updatedAt"].ToString();
            title = jsonObj["title"].ToString();
            publishKey = jsonObj["publishKey"].ToString();
            publishSecurity = jsonObj["publishSecurity"].ToString();
            disabled = (bool)jsonObj["disabled"];

            if (jsonObj["profiles"] != null)
            {
                Console.WriteLine("profiles--------" + jsonObj["profiles"].ToString());
                profiles = JsonConvert.DeserializeAnonymousType(jsonObj["profiles"].ToString(), profiles);
            }

            if (jsonObj["hosts"]["publish"] != null)
            {
                publishRtmpHost = jsonObj["hosts"]["publish"]["rtmp"].ToString();
            }
            if (jsonObj["hosts"]["live"] != null)
            {
                liveRtmpHost = jsonObj["hosts"]["live"]["rtmp"].ToString();
                liveHdlHost = jsonObj["hosts"]["live"]["hdl"].ToString();
                liveHlsHost = jsonObj["hosts"]["live"]["hls"].ToString();
                liveHttpHost = jsonObj["hosts"]["live"]["hls"].ToString();
            }
            if (jsonObj["hosts"]["playback"] != null)
            {
                playbackHttpHost = jsonObj["hosts"]["playback"]["hls"].ToString();
            }
            if (jsonObj["hosts"]["play"] != null)
            {
                playHttpHost = jsonObj["hosts"]["play"]["http"].ToString();
                playRtmpHost = jsonObj["hosts"]["play"]["rtmp"].ToString();

            }

            mStreamJsonStr = jsonObj.ToString();
        }



        public Stream(JObject jsonObject, Credentials credentials)
            : this(jsonObject)
        {
            mCredentials = credentials;
        }

        public string PlaybackHttpHost
        {
            get { return playbackHttpHost; }
        }

        public string PlayRtmpHost
        {
            get { return playRtmpHost; }
        }

        public string LiveHdlHost
        {
            get { return liveHdlHost; }

        }

        public string PlayHttpHost
        {
            get { return playHttpHost; }
        }
        public string LiveHttpHost
        {
            get { return liveHttpHost; }
        }


        public string LiveHlsHost
        {
            get { return liveHlsHost; }

        }
        public virtual string[] Profiles
        {
            get
            {
                return profiles;
            }
        }
        public virtual string PublishRtmpHost
        {
            get
            {
                return publishRtmpHost;
            }
        }
        public virtual string LiveRtmpHost
        {
            get
            {
                return liveRtmpHost;
            }
        }

        public virtual string StreamId
        {
            get
            {
                return id;
            }
        }
        public virtual string HubName
        {
            get
            {
                return hub;
            }
        }
        public virtual string CreatedAt
        {
            get
            {
                return createdAt;
            }
        }
        public virtual string UpdatedAt
        {
            get
            {
                return updatedAt;
            }
        }
        public virtual string Title
        {
            get
            {
                return title;
            }
        }
        public virtual string PublishKey
        {
            get
            {
                return publishKey;
            }
        }
        public virtual string PublishSecurity
        {
            get
            {
                return publishSecurity;
            }
        }
        public virtual bool Disabled
        {
            get
            {
                return disabled;
            }
        }

        public class Segment
        {
            private long start;
            private long end;

            public Segment(long start, long end)
            {
                this.start = start;
                this.end = end;
            }
            public virtual long Start
            {
                get
                {
                    return start;
                }
            }
            public virtual long End
            {
                get
                {
                    return end;
                }
            }
        }


        public class SaveAsResponse
        {
            private string url;
            private string targetUrl;
            private string persistentId;
            private string mJsonString;

            public SaveAsResponse(JObject jsonObj)
            {
                url = jsonObj["url"].ToString();
                try
                {
                    targetUrl = jsonObj["targetUrl"].ToString();
                    //

                }
                catch (System.NullReferenceException)
                {
                    // do nothing. ignore.
                }
                persistentId = jsonObj["persistentId"].ToString();
                mJsonString = jsonObj.ToString();
            }

            public virtual string Url
            {
                get
                {
                    return url;
                }
            }
            public virtual string TargetUrl
            {
                get
                {
                    return targetUrl;
                }
            }
            public virtual string PersistentId
            {
                get
                {
                    return persistentId;
                }
            }

            public override string ToString()
            {
                return mJsonString;
            }
        }

        public class SnapshotResponse
        {
            private string targetUrl;
            private string persistentId;
            private string mJsonString;
            public SnapshotResponse(JObject jsonObj)
            {
                targetUrl = jsonObj["targetUrl"].ToString();
                persistentId = jsonObj["persistentId"].ToString();
                mJsonString = jsonObj.ToString();
            }

            public virtual string TargetUrl
            {
                get
                {
                    return targetUrl;
                }
            }
            public virtual string PersistentId
            {
                get
                {
                    return persistentId;
                }
            }

            public override string ToString()
            {
                return mJsonString;
            }
        }

        public class FramesPerSecond
        {
            private float audio;
            private float video;
            private float data;
            public FramesPerSecond(float audio, float video, float data)
            {
                this.audio = audio;
                this.video = video;
                this.data = data;
            }

            public virtual float Audio
            {
                get
                {
                    return audio;
                }
            }
            public virtual float Video
            {
                get
                {
                    return video;
                }
            }
            public virtual float Data
            {
                get
                {
                    return data;
                }
            }
        }

        public class SegmentList
        {
            private IList<Segment> segmentList;



            public SegmentList(JObject jsonObj)
            {
                segmentList = new List<Segment>();
                JArray jlist = JArray.Parse(jsonObj["segments"].ToString());
                for (int i = 0; i < jlist.Count; ++i)
                {
                    JObject tempo = JObject.Parse(jlist[i].ToString());
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
            private string addr;
            private string status;
            private float bytesPerSecond;
            private FramesPerSecond framesPerSecond;
            private string startFrom;
            private string mJsonString;
            public Status(JObject jsonObj)
            {

                addr = jsonObj["addr"].ToString();
                status = jsonObj["status"].ToString();
                DateTime startFrominit = (DateTime)jsonObj["startFrom"];
                startFrom = startFrominit.ToString("yyyy-MM-ddTHH:mm:ssZ");
                try
                {
                    bytesPerSecond = (float)jsonObj["bytesPerSecond"];
                    float audio = (float)jsonObj["framesPerSecond"]["audio"];
                    float video = (float)jsonObj["framesPerSecond"]["video"];
                    float data = (float)jsonObj["framesPerSecond"]["data"];
                    framesPerSecond = new FramesPerSecond(audio, video, data);
                }
                catch (System.NullReferenceException e)
                {
                    Console.WriteLine(e.ToString());
                    Console.Write(e.StackTrace);
                }
                mJsonString = jsonObj.ToString();
            }
            public virtual string Addr
            {
                get
                {
                    return addr;
                }
            }
            public virtual string StartFrom
            {
                get
                {
                    return startFrom;
                }

            }
            public virtual string getStatus()
            {
                return status;
            }
            public virtual float BytesPerSecond
            {
                get
                {
                    return bytesPerSecond;
                }
            }
            public virtual FramesPerSecond FramesPerSecond
            {
                get
                {
                    return framesPerSecond;
                }
            }

            public override string ToString()
            {
                return mJsonString;
            }
        }

        public class StreamList
        {
            private string marker;
            private IList<Stream> itemList;
            public StreamList(JObject jsonObj, Credentials auth)
            {
                this.marker = jsonObj["marker"].ToString();
                Console.WriteLine("this.marker-----" + this.marker);

                try
                {
                    JToken record = jsonObj["items"];
                    itemList = new List<Stream>();
                    foreach (JObject jp in record)
                    {
                        itemList.Add(new Stream(JObject.Parse(jp.ToString()), auth));
                    }

                }
                catch (System.InvalidCastException e)
                {
                    Console.WriteLine(e.ToString());
                    Console.Write(e.StackTrace);
                }
            }

            public virtual string Marker
            {
                get
                {
                    return marker;
                }
            }
            public virtual IList<Stream> Streams
            {
                get
                {
                    return itemList;
                }
            }
        }


        public virtual Stream update(string publishKey, string publishSecrity, bool disabled)
        {
            return API.updateStream(mCredentials, this.id, publishKey, publishSecrity, disabled);
        }


        public virtual SegmentList segments()
        {
            return API.getStreamSegments(mCredentials, this.id, 0, 0, 0);
        }


        public virtual SegmentList segments(long start, long end)
        {
            return API.getStreamSegments(mCredentials, this.id, start, end, 0);
        }


        public virtual SegmentList segments(long start, long end, int limit)
        {
            return API.getStreamSegments(mCredentials, this.id, start, end, limit);
        }


        public virtual Status status()
        {
            return API.getStreamStatus(mCredentials, this.id);
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
            return API.hlsPlaybackUrl(this.mCredentials, this.id, start, end);
        }
        
        public virtual IDictionary<string, string> httpFlvLiveUrls()
        {
            return API.httpFlvLiveUrl(this);
        }


        public virtual string delete()
        {
            return API.deleteStream(mCredentials, this.id);
        }

        public virtual string toJsonString()
        {
            return mStreamJsonStr;
        }


        public virtual SaveAsResponse saveAs(string fileName, string format, long startTime, long endTime, string notifyUrl)
        {
            return API.saveAs(mCredentials, this.id, fileName, format, startTime, endTime, notifyUrl);
        }

        public virtual SaveAsResponse saveAs(string fileName, string format, long startTime, long endTime)
        {
            return saveAs(fileName, format, startTime, endTime, null);
        }

        public virtual SaveAsResponse saveAs(string fileName, string format, string notifyUrl)
        {
            return saveAs(fileName, format, 0, 0, notifyUrl);
        }
        public virtual SaveAsResponse saveAs(string fileName, string format)
        {
            return saveAs(fileName, format, 0, 0, null);
        }

        public virtual SnapshotResponse snapshot(string name, string format)
        {
            return API.snapshot(mCredentials, this.id, name, format, 0, null);
        }

        public virtual SnapshotResponse snapshot(string name, string format, string notifyUrl)
        {
            return API.snapshot(mCredentials, this.id, name, format, 0, notifyUrl);
        }

        public virtual SnapshotResponse snapshot(string name, string format, long time, string notifyUrl)
        {
            return API.snapshot(mCredentials, this.id, name, format, time, notifyUrl);
        }


        public virtual Stream enable()
        {
            return API.updateStream(mCredentials, this.id, null, null, false);
        }

        public virtual Stream disable()
        {
            return API.updateStream(mCredentials, this.id, null, null, true);
        }
    }


}