using Config = pili_sdk_csharp.pili_common.Config;
using Utils = pili_sdk_csharp.pili_common.Utils;


namespace pili_sdk_csharp.pili
{
    public class Configuration
    {
        private Configuration()
        {
        }

        private class ConfigurationHolder
        {
            public static readonly Configuration instance = new Configuration();
        }

        public static Configuration Instance
        {
            get
            {
                return ConfigurationHolder.instance;
            }
        }

        internal bool USE_HTTPS = Config.DEFAULT_USE_HTTPS;
        internal string API_HOST = Config.DEFAULT_API_HOST;
        internal string API_VERSION = Config.DEFAULT_API_VERSION;

        public virtual string APIHost
        {
            set
            {
                if (!Utils.isArgNotEmpty(value))
                {
                    throw new System.ArgumentException("Illegal API Host:" + value);
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
                    throw new System.ArgumentException("Illegal API Version:" + value);
                }
                API_VERSION = value;
            }
        }

        public void setHttpsEnabled(bool enabled)
        {
            USE_HTTPS = enabled;
        }
    }

}