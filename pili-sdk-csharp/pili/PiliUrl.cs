using pili_sdk_csharp.pili_qiniu;

namespace pili_sdk_csharp.pili
{
    public class PiliUrl
    {
        public static string RTMPPublishURL(Credentials credentials, string domain, string hubName, string key, long expireAfterSeconds)
        {
            var expire = DateTimeHelper.CurrentUnixTimeSeconds() + expireAfterSeconds;
            var token = credentials.SignStream(hubName, key, expire);
            const string defaultScheme = "rtmp";
            return $"{defaultScheme}://{domain}/{hubName}/{key}?e={expire}&token={token}";
        }

        public static string RTMPPlayURL(string domain, string hubName, string key)
        {
            return $"rtmp://{domain}/{hubName}/{key}";
        }

        public static string HLSPlayURL(string domain, string hubName, string key)
        {
            return $"http://{domain}/{hubName}/{key}.m3u8";
        }

        public static string HDLPlayURL(string domain, string hubName, string key)
        {
            return $"http://{domain}/{hubName}/{key}.flv";
        }

        public static string SnapshotPlayURL(string domain, string hubName, string key)
        {
            return $"http://{domain}/{hubName}/{key}.jpg";
        }
    }
}
