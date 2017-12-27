using Newtonsoft.Json;

namespace Qiniu.Pili.Meetings
{
    public class ActiveUser
    {
        [JsonProperty(PropertyName = "user_id")]
        public string UserId;

        [JsonProperty(PropertyName = "user_name")]
        public string UserName;
    }
}
