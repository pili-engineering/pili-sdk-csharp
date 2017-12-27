using Newtonsoft.Json;

namespace Qiniu.Pili.Meetings
{
    public class AllActiveUsers
    {
        [JsonProperty(PropertyName = "active_users")]
        public ActiveUser[] Users;
    }
}
