using System;
using Newtonsoft.Json;

namespace Qiniu.Pili.Meetings
{
    internal class RoomAccess
    {
        internal RoomAccess(string roomName, string userId, string perm, DateTime expireAt)
        {
            RoomName = roomName;
            UserId = userId;
            Perm = perm;
            ExpireAt = expireAt.Ticks / 1000; // seconds
            Version = "2.0";
        }

        internal RoomAccess(string roomName, string userId, string perm, DateTime expireAt, string version)
        {
            RoomName = roomName;
            UserId = userId;
            Perm = perm;
            ExpireAt = expireAt.Ticks / 1000;
            Version = version;
        }

        [JsonProperty(PropertyName = "expire_at")]
        internal long ExpireAt { get; set; }

        [JsonProperty(PropertyName = "perm")]
        internal string Perm { get; set; }

        [JsonProperty(PropertyName = "room_name")]
        internal string RoomName { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        internal string UserId { get; set; }

        [JsonProperty(PropertyName = "version")]
        internal string Version { get; set; }
    }
}
