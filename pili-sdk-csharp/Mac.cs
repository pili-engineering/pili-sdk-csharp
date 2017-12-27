using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Qiniu.Pili.Internal;

namespace Qiniu.Pili
{
    internal sealed class Mac
    {
        private readonly string _accessKey;
        private readonly string _secretKey;
        private readonly HMACSHA1 _skSpec;

        public Mac(string accessKey, string secretKey)
        {
            _accessKey = accessKey;
            _secretKey = secretKey;
            _skSpec = new HMACSHA1(Encoding.UTF8.GetBytes(secretKey));
        }

        public string Sign(string data)
        {
            var sum = _skSpec.ComputeHash(data);
            var sign = Base64UrlEncoder.Encode(sum);
            return _accessKey + ":" + sign;
        }

        public string SignRequest(Uri uri, HttpMethod method, byte[] body, string contentType)
        {
            var sb = new StringBuilder();

            sb.Append($"{method} {uri.PathAndQuery}");
            sb.Append($"\nHost: {uri.Host}");

            if (uri.Port > 0)
            {
                sb.Append($":{uri.Port:D}");
            }

            if (contentType != null)
            {
                sb.Append($"\nContent-Type: {contentType}");
            }

            // body
            sb.Append("\n\n");
            if (IncBody(body, contentType))
            {
                sb.Append(Encoding.UTF8.GetString(body));
            }

            var sum = _skSpec.ComputeHash(sb.ToString());
            var sign = Base64UrlEncoder.Encode(sum);
            return _accessKey + ":" + sign;
        }

        private static bool IncBody(byte[] body, string contentType)
        {
            const int maxContentLength = 1024 * 1024;
            var typeOk = contentType != null && !contentType.Equals("application/octet-stream");
            var lengthOk = body != null && body.Length > 0 && body.Length < maxContentLength;
            return typeOk && lengthOk;
        }

        /// <summary>
        ///     连麦 RoomToken
        /// </summary>
        public string SignRoomToken(string roomAccess)
        {
            var encodedRoomAcc = Base64UrlEncoder.Encode(roomAccess);
            var sign = _skSpec.ComputeHash(encodedRoomAcc);
            var encodedSign = Base64UrlEncoder.Encode(sign);
            return _accessKey + ":" + encodedSign + ":" + encodedRoomAcc;
        }
    }
}
