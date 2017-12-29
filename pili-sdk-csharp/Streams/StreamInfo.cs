using Newtonsoft.Json;

namespace Qiniu.Pili.Streams
{
    public sealed class StreamInfo
    {
        internal StreamInfo()
        {
        }

        internal StreamInfo(string hub, string key)
        {
            SetMeta(hub, key);
        }

        [JsonProperty]
        public string Hub { get; private set; }

        [JsonProperty]
        public long DisabledTill { get; private set; }

        [JsonProperty]
        public string Key { get; private set; }

        [JsonProperty]
        public string[] Converts { get; private set; }

        internal void SetMeta(string hub, string key)
        {
            Key = key;
            Hub = hub;
        }
    }
}
