using Newtonsoft.Json;

namespace Qiniu.Pili.Streams
{
    internal class ConvertsOptions
    {
        public ConvertsOptions(string[] converts)
        {
            Converts = converts;
        }

        [JsonProperty]
        private string[] Converts { get; set; }
    }
}
