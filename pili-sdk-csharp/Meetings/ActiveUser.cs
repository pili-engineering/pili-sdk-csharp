using Newtonsoft.Json;

namespace Qiniu.Pili.Meetings
{
    public class ActiveUser
    {
        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "user_name")]
        public string UserName { get; set; }
    }
}
