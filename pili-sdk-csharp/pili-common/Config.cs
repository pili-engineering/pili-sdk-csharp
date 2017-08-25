using System;

namespace pili_sdk_csharp.pili_common
{
    public class Config
    {
        public const string SdkVersion = "1.5.0";

        public const string UserAgent = "pili-sdk-csharp";

        public const string Utf8 = "UTF-8";

        public const string DefaultAPIVersion = "v1";

        [Obsolete]
        public const string APIVersion = DefaultAPIVersion;

        public const string DefaultAPIHost = "pili.qiniuapi.com";
        public const bool DefaultUseHttps = false;

        public const int TitleMinLength = 5;
        public const int TitleMaxLength = 200;
    }
}
