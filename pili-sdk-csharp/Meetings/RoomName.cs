using Newtonsoft.Json;

namespace Qiniu.Pili.Meetings
{
    internal class RoomName
    {
        [JsonProperty(PropertyName = "room_name")]
        internal string Name { get; set; }
    }
}
