using System;

namespace pili_sdk_csharp.pili_common
{
    public class UrlSafeBase64
    {
        public static string EncodeToString(string data)
        {
            try
            {
                return EncodeToString(data.GetBytes(Config.Utf8));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
            return null;
        }

        public static string EncodeToString(byte[] data)
        {
            return Base64.EncodeToString(data, Base64.UrlSafe | Base64.NoWrap);
        }

        public static byte[] Decode(string data)
        {
            return Base64.Decode(data, Base64.UrlSafe | Base64.NoWrap);
        }
    }
}
