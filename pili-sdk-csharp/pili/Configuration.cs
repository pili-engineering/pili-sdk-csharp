using System;
using pili_sdk_csharp.pili_common;

namespace pili_sdk_csharp.pili
{
    public class Configuration
    {
        public static readonly Configuration Instance = new Configuration();
        internal string API_HOST = Config.DefaultAPIHost;
        internal string API_VERSION = Config.DefaultAPIVersion;

        internal bool UseHttps = Config.DefaultUseHttps;

        private Configuration()
        {
        }

        public virtual string APIHost
        {
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Illegal API Host:" + value);
                }
                API_HOST = value;
            }
        }

        public virtual string APIVersion
        {
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Illegal API Version:" + value);
                }
                API_VERSION = value;
            }
        }

        public void SetHttpsEnabled(bool enabled)
        {
            UseHttps = enabled;
        }
    }
}
