using Newtonsoft.Json;

namespace Qiniu.Pili.Streams
{
    internal class HistoryRet
    {
        [JsonProperty]
        public Record[] Items { get; set; }
    }
}
