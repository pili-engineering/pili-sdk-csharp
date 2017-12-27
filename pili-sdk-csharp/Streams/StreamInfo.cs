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

        public string Hub { get; internal set; }

        public long DisabledTill { get; internal set; }

        public string Key { get; internal set; }

        public string[] Converts { get; internal set; }

        internal void SetMeta(string hub, string key)
        {
            Key = key;
            Hub = hub;
        }
    }
}
