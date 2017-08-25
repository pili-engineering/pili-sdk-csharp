using System;
using System.IO;
using Newtonsoft.Json;

namespace pili_sdk_csharp.pili_common
{
    public class Utils
    {
        public static int BufferLen = 32 * 1024;

        public static string UserAgent
        {
            get
            {
                var csharpVersion = "csharp";
                var os = "windows";
                var sdk = Config.UserAgent + Config.SdkVersion;
                return sdk + os + csharpVersion;
            }
        }

        public static string JsonEncode(object obj)
        {
            var setting = new JsonSerializerSettings();
            setting.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.SerializeObject(obj, setting);
        }

        public static T ToObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static void Copy(Stream dst, Stream src)
        {
            var l = src.Position;
            var buffer = new byte[BufferLen];
            while (true)
            {
                var n = src.Read(buffer, 0, BufferLen);
                if (n == 0) break;
                dst.Write(buffer, 0, n);
            }
            src.Seek(l, SeekOrigin.Begin);
        }

        public static void CopyN(Stream dst, Stream src, long numBytesToCopy)
        {
            var l = src.Position;
            var buffer = new byte[BufferLen];
            long numBytesWritten = 0;
            while (numBytesWritten < numBytesToCopy)
            {
                var len = BufferLen;
                if (numBytesToCopy - numBytesWritten < len)
                {
                    len = (int)(numBytesToCopy - numBytesWritten);
                }
                var n = src.Read(buffer, 0, len);
                if (n == 0) break;
                dst.Write(buffer, 0, n);
                numBytesWritten += n;
            }
            src.Seek(l, SeekOrigin.Begin);
            if (numBytesWritten != numBytesToCopy)
            {
                throw new Exception("StreamUtil.CopyN: nwritten not equal to ncopy");
            }
        }

        /*
         * check the arg.
         * 1. arg == null, return false, treat as empty situation
         * 2. arg == "", return false, treat as empty situation
         * 3. arg == " " or arg == "   ", return false, treat as empty situation
         * 4. return true, only if the arg is a illegal string
         *
         * */
        public static bool IsArgNotEmpty(string arg)
        {
            return arg != null && arg.Trim().Length > 0;
        }
    }
}
