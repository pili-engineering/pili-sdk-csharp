using System;
using System.Security.Cryptography;
using System.Text;
using pili_sdk_csharp.pili_common;

namespace pili_sdk_csharp.pili_qiniu
{
    public class Credentials
    {
        private const string DigestAuthPrefix = "Qiniu";
        private readonly string _accessKey;
        private readonly string _secretKey;
        private readonly HMACSHA1 _skSpec;

        public Credentials(string ak, string sk)
        {
            _accessKey = ak ?? throw new ArgumentNullException(nameof(ak), "Invalid accessKey!");
            _secretKey = sk ?? throw new ArgumentNullException(nameof(sk), "Invalid secretKey!!");
            try
            {
                _skSpec = new HMACSHA1(Encoding.UTF8.GetBytes(_secretKey));
            }
            catch (Exception e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }

        public virtual string SignStream(string hubName, string streamKey, long expire)
        {
            var path = $"/{hubName}/{streamKey}?e={expire}";
            return $"{_accessKey}:{SignData(path)}";
        }

        public virtual string SignRequest(Uri url, string method, byte[] body, string contentType)
        {
            var sb = new StringBuilder();

            // <Method> <Path><?Query>
            var line = $"{method} {url.LocalPath}";
            sb.Append(line);
            if (url.Query != "")
            {
                sb.Append(url.Query);
            }

            // Host: <Host>
            sb.Append($"\nHost: {url.Host}");
            if (url.Port != 80)
            {
                sb.Append($":{url.Port}");
            }

            // Content-Type: <Content-Type>
            if (contentType != null)
            {
                sb.Append($"\nContent-Type: {contentType}");
            }

            // body
            sb.Append("\n\n");
            if (body != null && contentType != null && contentType != "application/octet-stream")
            {
                sb.Append(Encoding.UTF8.GetString(body));
            }
            return $"{DigestAuthPrefix} {_accessKey}:{SignData(sb.ToString())}";
        }

        private static byte[] Digest(string secret, string data)
        {
            try
            {
                var encoding = Encoding.UTF8;
                var newdata = encoding.GetBytes(data);
                var bytesSk = encoding.GetBytes(secret);
                var mac = new HMACSHA1(bytesSk);
                var digest = mac.ComputeHash(newdata);

                return digest;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                throw new Exception("Failed to digest: " + e.Message);
            }
        }

        public static string Sign(string secret, string data)
        {
            return UrlSafeBase64.EncodeToString(Digest(secret, data));
        }

        private string SignData(string data)
        {
            string sign;
            try
            {
                var encoding = Encoding.UTF8;
                var newdata = encoding.GetBytes(data);
                var digest = _skSpec.ComputeHash(newdata);
                sign = UrlSafeBase64.EncodeToString(digest);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to generate HMAC : " + e.Message);
            }
            return sign;
        }
    }
}
