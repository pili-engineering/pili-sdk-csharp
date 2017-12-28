using Newtonsoft.Json;

namespace Qiniu.Pili.Meetings
{
    internal class CreateArgs
    {
        public CreateArgs(string ownerId, string room, int userMax)
        {
            OwnerId = ownerId;
            Room = room;
            UserMax = userMax;
        }

        public CreateArgs(string ownerId, string room)
        {
            OwnerId = ownerId;
            Room = room;
        }

        public CreateArgs(string ownerId)
        {
            OwnerId = ownerId;
        }

        [JsonProperty(PropertyName = "owner_id")]
        internal string OwnerId { get; set; }

        [JsonProperty(PropertyName = "room_name")]
        internal string Room { get; set; }

        [JsonProperty(PropertyName = "user_max")]
        internal int UserMax { get; set; }
    }
}
