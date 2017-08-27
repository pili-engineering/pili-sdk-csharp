using System;
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

        public virtual Stream CreateStream()
        {
            return API.CreateStream(_mCredentials, _mHubName, null, null, null);
        }

        public virtual Stream CreateStream(string title, string publishKey, string publishSecurity)
        {
            return API.CreateStream(_mCredentials, _mHubName, title, publishKey, publishSecurity);
        }

        public virtual Stream GetStream(string streamId)
        {
            return API.GetStream(_mCredentials, streamId);
        }

        public virtual Stream.StreamList ListStreams()
        {
            return API.ListStreams(_mCredentials, _mHubName, null, 0, null);
        }

        public virtual Stream.StreamList ListStreams(string marker, long limit)
        {
            return API.ListStreams(_mCredentials, _mHubName, marker, limit, null);
        }

        public virtual Stream.StreamList ListStreams(string marker, long limit, string titlePrefix)
        {
            return API.ListStreams(_mCredentials, _mHubName, marker, limit, titlePrefix);
        }
    }
}
