using System;

namespace pili_sdk_csharp.pili_common
{

    public class Config
    {
        public const string SDK_VERSION = "1.5.0";

        public const string USER_AGENT = "pili-sdk-csharp";

        public const string UTF8 = "UTF-8";

        public const string DEFAULT_API_VERSION = "v1";

        [Obsolete]
        public const string API_VERSION = DEFAULT_API_VERSION;

        public const string DEFAULT_API_HOST = "pili.qiniuapi.com";
        public const bool DEFAULT_USE_HTTPS = false;

        public const int TITLE_MIN_LENGTH = 5;
        public const int TITLE_MAX_LENGTH = 200;
    }

}