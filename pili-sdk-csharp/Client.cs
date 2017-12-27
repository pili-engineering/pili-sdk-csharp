using System;

#if NET45
using Qiniu.Pili.Internal;
#endif

namespace Qiniu.Pili
{
    public sealed class Client
    {
        private readonly RPC _cli;

        public Client(string accessKey, string secretKey)
        {
            _cli = new RPC(new Mac(accessKey, secretKey));
        }

        /// <summary>
        ///     Generate RTMP publish URL.
        /// </summary>
        /// <param name="expireAfterSeconds">URL will be invalid after expireAfterSeconds</param>
        /// <returns>RTMP publish URL</returns>
        public string RTMPPublishURL(string domain, string hub, string streamKey, int expireAfterSeconds)
        {
            var expire = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + expireAfterSeconds;
            var path = $"/{hub}/{streamKey}?e={expire:D}";
            string token;
            try
            {
                token = _cli.Mac.Sign(path);
            }
            catch (Exception)
            {
                return null;
            }

            return $"rtmp://{domain}{path}&token={token}";
        }

        /// <summary>
        ///     RTMPPlayURL generates RTMP play URL
        /// </summary>
        public string RTMPPlayURL(string domain, string hub, string streamKey)
        {
            return $"rtmp://{domain}/{hub}/{streamKey}";
        }

        /// <summary>
        ///     HLSPlayURL generates HLS play URL
        /// </summary>
        public string HLSPlayURL(string domain, string hub, string streamKey)
        {
            return $"http://{domain}/{hub}/{streamKey}.m3u8";
        }

        /// <summary>
        ///     HDLPlayURL generates HDL play URL
        /// </summary>
        public string HDLPlayURL(string domain, string hub, string streamKey)
        {
            return $"http://{domain}/{hub}/{streamKey}.flv";
        }

        /// <summary>
        ///     SnapshotPlayURL generates snapshot URL
        /// </summary>
        public string SnapshotPlayURL(string domain, string hub, string streamKey)
        {
            return $"http://{domain}/{hub}/{streamKey}.jpg";
        }

        public Hub NewHub(string hub)
        {
            return new Hub(_cli, hub);
        }

        public Meeting NewMeeting()
        {
            return new Meeting(_cli);
        }
    }
}
