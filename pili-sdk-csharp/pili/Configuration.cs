using System;
using pili_sdk_csharp.pili_common;

namespace pili_sdk_csharp.pili
{
    public class Configuration
    {
        internal string API_HOST = Config.DEFAULT_API_HOST;
        internal string API_VERSION = Config.DEFAULT_API_VERSION;

        internal bool USE_HTTPS = Config.DEFAULT_USE_HTTPS;

        private Configuration()
        {
        }

        public static Configuration Instance => ConfigurationHolder.instance;

        public virtual string APIHost
        {
            set
            {
                if (!Utils.isArgNotEmpty(value))
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
                if (!Utils.isArgNotEmpty(value))
                {
                    throw new ArgumentException("Illegal API Version:" + value);
                }
                API_VERSION = value;
            }
        }

        public void setHttpsEnabled(bool enabled)
        {
            USE_HTTPS = enabled;
        }

        private class ConfigurationHolder
        {
            public static readonly Configuration instance = new Configuration();
        }
    }
}
