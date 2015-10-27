using pili_sdk_csharp.pili_common;
using System;
namespace pili_sdk_csharp.pili_common
{

    public class UrlSafeBase64
    {

        public static string encodeToString(string data)
        {
            try
            {
                return encodeToString(data.GetBytes(Config.UTF8));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
            return null;
        }

        public static string encodeToString(byte[] data)
        {
            return Base64.encodeToString(data, Base64.URL_SAFE | Base64.NO_WRAP);
        }

        public static byte[] decode(string data)
        {
            return Base64.decode(data, Base64.URL_SAFE | Base64.NO_WRAP);
        }
    }

}