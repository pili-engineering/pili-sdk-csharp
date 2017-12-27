using System;
using Newtonsoft.Json;

namespace Qiniu.Pili.Meetings
{
    internal class RoomAccess
    {
        [JsonProperty(PropertyName = "expire_at")]
        internal long ExpireAt;

        [JsonProperty(PropertyName = "perm")]
        internal string Perm;

        [JsonProperty(PropertyName = "room_name")]
        internal string RoomName;

        [JsonProperty(PropertyName = "user_id")]
        internal string UserId;

        [JsonProperty(PropertyName = "version")]
        internal string Version;

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
    }
}
