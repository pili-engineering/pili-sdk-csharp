using Newtonsoft.Json;

namespace Qiniu.Pili.Meetings
{
    internal class CreateArgs
    {
        [JsonProperty(PropertyName = "owner_id")]
        internal string OwnerId;

        [JsonProperty(PropertyName = "room_name")]
        internal string Room;

        [JsonProperty(PropertyName = "user_max")]
        internal int UserMax;

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
    }
}
