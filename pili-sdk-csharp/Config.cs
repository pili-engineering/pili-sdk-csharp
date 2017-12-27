using System.Runtime.InteropServices;

namespace Qiniu.Pili
{
    public sealed class Config
    {
        internal const string APIHttpScheme = "http://";

        public const string Version = "2.1.0-alpha";

        internal static readonly string APIUserAgent =
            $"pili-sdk-csharp/{Version} {RuntimeInformation.FrameworkDescription} {RuntimeInformation.OSDescription}/{RuntimeInformation.OSArchitecture}";

        public static string APIHost { get; set; } = "pili.qiniuapi.com";

        public static string RTCAPIHost { get; set; } = "rtc.qiniuapi.com";
    }
}
