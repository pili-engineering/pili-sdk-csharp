﻿using System;
using System.Security.Cryptography;
using System.Text;
using pili_sdk_csharp.pili_common;

namespace pili_sdk_csharp.pili_qiniu
{
    public class Credentials
    {
        private const string DIGEST_AUTH_PREFIX = "Qiniu";
        private readonly string mAccessKey;
        private readonly string mSecretKey;
        private readonly HMACSHA1 mSkSpec;

        public Credentials(string ak, string sk)
        {
            if (ak == null || sk == null)
            {
                throw new ArgumentException("Invalid accessKey or secretKey!!");
            }
            mAccessKey = ak;
            mSecretKey = sk;
            try
            {
                mSkSpec = new HMACSHA1(mSecretKey.GetBytes(Config.UTF8));
            }
            catch (Exception e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }

        public virtual string signRequest(Uri url, string method, byte[] body, string contentType)
        {
            var sb = new StringBuilder();

            // <Method> <Path><?Query>
            var line = string.Format("{0} {1}", method, url.LocalPath);
            sb.Append(line);
            if (url.Query != "")
            {
                sb.Append(url.Query);
            }

            // Host: <Host>
            sb.Append(string.Format("\nHost: {0}", url.Host));


            if (url.Port != 80)
            {
                sb.Append(string.Format(":{0}", url.Port));
            }
            // Content-Type: <Content-Type>
            if (contentType != null)
            {
                sb.Append(string.Format("\nContent-Type: {0}", contentType));
            }

            // body
            sb.Append("\n\n");
            if (body != null && contentType != null && !"application/octet-stream".Equals(contentType))
            {
                sb.Append(StringHelperClass.NewString(body));
            }
            return string.Format("{0} {1}:{2}", DIGEST_AUTH_PREFIX, mAccessKey, signData(sb.ToString()));
        }

        private static byte[] digest(string secret, string data)
        {
            try
            {
                var encoding = Encoding.UTF8;
                var newdata = encoding.GetBytes(data);
                var bytesSK = encoding.GetBytes(secret);
                var mac = new HMACSHA1(bytesSK);
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


        public static string sign(string secret, string data)
        {
            return UrlSafeBase64.encodeToString(digest(secret, data));
        }

        private string signData(string data)
        {
            string sign = null;
            try
            {
                var mac = createMac(mSkSpec);
                var encoding = Encoding.UTF8;
                var newdata = encoding.GetBytes(data);
                var digest = mac.ComputeHash(newdata);
                sign = UrlSafeBase64.encodeToString(digest);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to generate HMAC : " + e.Message);
            }
            return sign;
        }

        private static HMACSHA1 createMac(HMACSHA1 secretKeySpec)
        {
            var mac = secretKeySpec;
            return mac;
        }
    }
}
