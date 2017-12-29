using Newtonsoft.Json;

namespace Qiniu.Pili.Meetings
{
    public class Room
    {
        [JsonProperty(PropertyName = "room_name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "owner_id")]
        public string OwnerId { get; set; }

        [JsonProperty(PropertyName = "room_status")]
        public Status Status { get; set; }

        [JsonProperty(PropertyName = "user_max")]
        public int UserMaxe { get; set; }
    }
}
