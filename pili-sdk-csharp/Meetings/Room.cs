using Newtonsoft.Json;

namespace Qiniu.Pili.Meetings
{
    public class Room
    {
        [JsonProperty(PropertyName = "room_name")]
        public string Name;

        [JsonProperty(PropertyName = "owner_id")]
        public string OwnerId;

        [JsonProperty(PropertyName = "room_status")]
        public Status Status;

        [JsonProperty(PropertyName = "user_max")]
        public int UserMaxe;
    }
}
