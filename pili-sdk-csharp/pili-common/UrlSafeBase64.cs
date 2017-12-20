using System;
using System.Text;

namespace pili_sdk_csharp.pili_common
{
    public class UrlSafeBase64
    {
        public static string EncodeToString(string data)
        {
            try
            {
                return EncodeToString(Encoding.UTF8.GetBytes(data));
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
            return Convert.ToBase64String(data)
                .Replace('+', '-').Replace('/', '_')
                .TrimEnd('=');
        }

        public static string Base64UrlEncode(byte[] data)
        {
            return Convert.ToBase64String(data)
                .Replace('+', '-').Replace('/', '_');
        }

        public static byte[] Decode(string base64)
        {
            base64 = base64
                .PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=')
                .Replace('_', '/').Replace('-', '+');

            return Convert.FromBase64String(base64);
        }
    }
}
