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
                const string csharpVersion = "csharp";
                const string os = "windows";
                const string sdk = Config.UserAgent + Config.SdkVersion;
                return sdk + os + csharpVersion;
            }
        }

        public static string JsonEncode(object obj)
        {
            var setting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            return JsonConvert.SerializeObject(obj, setting);
        }

        public static T ToObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
