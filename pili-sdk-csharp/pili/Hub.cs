using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using pili_sdk_csharp.pili_common;
using pili_sdk_csharp.pili_qiniu;

namespace pili_sdk_csharp.pili
{
    public class Hub
    {
        private readonly Credentials _mCredentials;
        private readonly string _mHubName;

        public Hub(Credentials credentials, string hubName)
        {
            _mCredentials = credentials ?? throw new ArgumentException(MessageConfig.NullCredentialsExceptionMsg);
            _mHubName = hubName ?? throw new ArgumentException(MessageConfig.NullHubnameExceptionMsg);
        }

        public virtual Stream CreateStream(string streamId)
        {
            return API.CreateStream(_mCredentials, _mHubName, streamId);
        }

        public virtual Stream GetStream(string streamId)
        {
            return new Stream(_mHubName, streamId, _mCredentials);
        }

        public virtual List<LiveStatusWithKey> BatchLiveStatus(List<string> streamKeys)
        {
            return API.BatchLiveStatus(_mCredentials, _mHubName, streamKeys)["items"].ToObject<List<LiveStatusWithKey>>();
        }

        public virtual StreamList List(string prefix, int limit, string marker)
        {
            return GetList(false, prefix, limit, marker);
        }

        public virtual StreamList ListLive(string prefix, int limit, string marker)
        {
            return GetList(true, prefix, limit, marker);
        }

        private StreamList GetList(bool live, string prefix, int limit, string marker)
        {
            return new StreamList(API.StreamList(_mCredentials, _mHubName, live, prefix, limit, marker));
        }
    }

    public class LiveStatusWithKey
    {
        public string Key { get; set; }
        public Stream.StreamStatus LiveStatus { get; set; }
    }

    public class StreamList
    {
        public StreamList(JObject jsonObj)
        {
            Keys = new List<string>();
            foreach (var item in jsonObj["items"])
            {
                foreach (var kv in item)
                {
                    var key = kv.First().ToString();
                    Keys.Add(key);
                }
            }

            Marker = jsonObj["marker"].ToString();
        }

        public List<string> Keys { get; set; }
        public string Marker { get; set; }
    }
}
