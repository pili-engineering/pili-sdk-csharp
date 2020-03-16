using pili_sdk_csharp.pili_qiniu;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using pili_sdk_csharp.pili_common;
namespace pili_sdk_csharp.rtc
{
    public class RtcManager
    {
        private Credentials cre;
        private async Task<HttpResponseMessage> Post(Uri url, string contentType, string body)
        {
            var content = new StringContent(body, Encoding.UTF8, contentType);
            content.Headers.ContentType.CharSet = "";
            HttpClient client = new HttpClient();
            String token = cre.signRequest(url, "POST", body.GetBytes(Config.UTF8), contentType);
            client.DefaultRequestHeaders.Add("Authorization", token);
            var response = await client.PostAsync(url, content);
            return response;
        }

        private async Task<HttpResponseMessage> Get(Uri url)
        {

            HttpClient client = new HttpClient();
            String token = cre.signRequest(url, "GET", null, null);
            client.DefaultRequestHeaders.Add("Authorization", token);
            var response = await client.GetAsync(url);
            return response;
        }
        private async Task<HttpResponseMessage> Delete(Uri url)
        {

            HttpClient client = new HttpClient();
            String token = cre.signRequest(url, "DELETE", null, null);
            client.DefaultRequestHeaders.Add("Authorization", token);
            var response = await client.DeleteAsync(url);
            return response;
        }

        public RtcManager(Credentials cre)
        {
            this.cre = cre;
        }


        public HttpResponseMessage createApp(String hub, String title, int maxUsers, bool noAutoKickUser)
        {
            Uri url = new Uri("http://rtc.qiniuapi.com/v3/apps");
            JObject jObj = new JObject();
            if (hub != null) jObj.Add("hub", hub);
            if (title != null) jObj.Add("title", title);
            if (hub != null) jObj.Add("maxUsers", maxUsers);
            jObj.Add("noAutoKickUser", noAutoKickUser);

            return Post(url, "application/json", jObj.ToString()).Result;

        }

        public HttpResponseMessage getApp(String appId)
        {
            Uri url = new Uri("http://rtc.qiniuapi.com/v3/apps/" + appId);
            return Get(url).Result;
        }

        public HttpResponseMessage deleteApp(String appId)
        {
            Uri url = new Uri("http://rtc.qiniuapi.com/v3/apps/" + appId);
            return Delete(url).Result;
        }

        public HttpResponseMessage updateApp(string appId, string hub, string title, int maxUsers, bool noAutoKickUser)
        {
            Uri url = new Uri("http://rtc.qiniuapi.com/v3/apps/" + appId);
            JObject jObj = new JObject();
            if (hub != null) jObj.Add("hub", hub);
            if (title != null) jObj.Add("title", title);
            if (hub != null) jObj.Add("maxUsers", maxUsers);
            jObj.Add("noAutoKickUser", noAutoKickUser);

            return Post(url, "application/json", jObj.ToString()).Result;
        }

        public HttpResponseMessage listUser(string appId, string roomName)
        {
            Uri url = new Uri(String.Format("http://rtc.qiniuapi.com/v3/apps/{0}/rooms/{1}/users", appId, roomName));
            return Get(url).Result;
        }

        public HttpResponseMessage kickUser(string appId, string roomName, string userId)
        {
            Uri url = new Uri(String.Format("http://rtc.qiniuapi.com/v3/apps/{0}/rooms/{1}/users/{2}", appId, roomName, userId));
            return Delete(url).Result;
        }
        public HttpResponseMessage listActiveRooms(String appId, String prefix, int offset, int limit)
        {
            Uri url = new Uri(String.Format("http://rtc.qiniuapi.com/v3/apps/{0}/rooms?prefix={1}&offset{2}&limit={3}", appId, prefix, offset, limit));
            return Get(url).Result;
        }
        
    }
}