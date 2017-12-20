using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pili_sdk_csharp.pili_common;
using pili_sdk_csharp.pili_qiniu;

namespace pili_sdk_csharp.pili
{
    public static class API
    {
        private const string ContentType = "application/json";

        private static readonly string APIBaseUrl =
            $"{(Configuration.Instance.UseHttps ? "https" : "http")}://{Configuration.Instance.API_HOST}/{Configuration.Instance.API_VERSION}";

        public static Stream CreateStream(Credentials credentials, string hubName, string streamId)
        {
            //  System.out.println("createStream:" + API_BASE_URL);
            var urlStr = APIBaseUrl + $"/hubs/{hubName}/streams";

            var json = new Dictionary<string, string>
            {
                { "key", streamId }
            };
            HttpWebResponse response = null;
            try
            {
                var url = new Uri(urlStr);
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                var jsonobj = JsonConvert.SerializeObject(json);
                var body = Encoding.UTF8.GetBytes(jsonobj);
                var macToken = credentials.SignRequest(url, "POST", body, ContentType);
                request.ContentType = ContentType;
#if NET45
                request.UserAgent = Utils.UserAgent;
                request.Headers.Add("Authorization", macToken);
#else
                request.Headers["User-Agent"] = Utils.UserAgent;
                request.Headers["Authorization"] = macToken;
#endif

                using (var requestStream = request.GetRequestStreamAsync().Result)
                {
                    new MemoryStream(body).CopyTo(requestStream);
                }

                response = (HttpWebResponse)request.GetResponseAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                throw new PiliException(e);
            }

            // response never be null
            if ((int)response.StatusCode == 200)
            {
                return new Stream(hubName, streamId, credentials);
            }

            throw new PiliException(response);
        }

        // Get an exist stream
        public static JObject GetStream(Credentials credentials, string hubName, string ekey)
        {
            var urlStr = $"{APIBaseUrl}/hubs/{hubName}/streams/{ekey}";
            HttpWebResponse response = null;
            try
            {
                var url = new Uri(urlStr);
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                var macToken = credentials.SignRequest(url, "GET", null, null);
#if NET45
                request.UserAgent = Utils.UserAgent;
                request.Headers.Add("Authorization", macToken);
#else
                request.Headers["User-Agent"] = Utils.UserAgent;
                request.Headers["Authorization"] = macToken;
#endif
                response = (HttpWebResponse)request.GetResponseAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                throw new PiliException(e);
            }

            // response never be null
            if ((int)response.StatusCode == 200)
            {
                try
                {
                    var reader = new StreamReader(response.GetResponseStream());
                    var text = reader.ReadToEnd();
                    return JObject.Parse(text);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.ToString());
                    Console.Write(e.StackTrace);
                    throw new PiliException(e);
                }
            }

            throw new PiliException(response);
        }

        public static void SetDisableTill(Credentials credentials, string hubName, string ekey, long till)
        {
            var urlStr = $"{APIBaseUrl}/hubs/{hubName}/streams/{ekey}/disabled";
            var json = new Dictionary<string, long>
            {
                { "disabledTill", till }
            };
            HttpWebResponse response = null;
            try
            {
                var url = new Uri(urlStr);
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                var jsonobj = JsonConvert.SerializeObject(json);
                var body = Encoding.UTF8.GetBytes(jsonobj);
                var macToken = credentials.SignRequest(url, "POST", body, ContentType);
                request.ContentType = ContentType;
#if NET45
                request.UserAgent = Utils.UserAgent;
                request.Headers.Add("Authorization", macToken);
#else
                request.Headers["User-Agent"] = Utils.UserAgent;
                request.Headers["Authorization"] = macToken;
#endif

                using (var requestStream = request.GetRequestStreamAsync().Result)
                {
                    new MemoryStream(body).CopyTo(requestStream);
                }

                response = (HttpWebResponse)request.GetResponseAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                throw new PiliException(e);
            }
        }

        public static JObject LiveStatus(Credentials credentials, string hubName, string ekey)
        {
            var urlStr = $"{APIBaseUrl}/hubs/{hubName}/streams/{ekey}/live";
            HttpWebResponse response = null;
            try
            {
                var url = new Uri(urlStr);
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                var macToken = credentials.SignRequest(url, "GET", null, null);
#if NET45
                request.UserAgent = Utils.UserAgent;
                request.Headers.Add("Authorization", macToken);
#else
                request.Headers["User-Agent"] = Utils.UserAgent;
                request.Headers["Authorization"] = macToken;
#endif
                response = (HttpWebResponse)request.GetResponseAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                throw new PiliException(e);
            }

            // response never be null
            if ((int)response.StatusCode == 200)
            {
                try
                {
                    var reader = new StreamReader(response.GetResponseStream());
                    var text = reader.ReadToEnd();
                    return JObject.Parse(text);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.ToString());
                    Console.Write(e.StackTrace);
                    throw new PiliException(e);
                }
            }

            throw new PiliException(response);
        }

        public static JObject SaveAs(Credentials credentials, string hubName, string ekey, Stream.SaveasOptions options)
        {
            if (options == null)
            {
                options = new Stream.SaveasOptions();
            }

            var urlStr = $"{APIBaseUrl}/hubs/{hubName}/streams/{ekey}/saveas";
            HttpWebResponse response = null;
            try
            {
                var url = new Uri(urlStr);
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                var jsonobj = JsonConvert.SerializeObject(options);
                var body = Encoding.UTF8.GetBytes(jsonobj);
                var macToken = credentials.SignRequest(url, "POST", body, ContentType);
                request.ContentType = ContentType;
#if NET45
                request.UserAgent = Utils.UserAgent;
                request.Headers.Add("Authorization", macToken);
#else
                request.Headers["User-Agent"] = Utils.UserAgent;
                request.Headers["Authorization"] = macToken;
#endif

                using (var requestStream = request.GetRequestStreamAsync().Result)
                {
                    new MemoryStream(body).CopyTo(requestStream);
                }

                response = (HttpWebResponse)request.GetResponseAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                throw new PiliException(e);
            }

            // response never be null
            if ((int)response.StatusCode == 200)
            {
                try
                {
                    var reader = new StreamReader(response.GetResponseStream());
                    var text = reader.ReadToEnd();
                    return JObject.Parse(text);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.ToString());
                    Console.Write(e.StackTrace);
                    throw new PiliException(e);
                }
            }

            throw new PiliException(response);
        }

        public static JObject Snapshot(Credentials credentials, string hubName, string ekey, Stream.SnapshotOptions options)
        {
            if (options == null)
            {
                options = new Stream.SnapshotOptions();
            }

            var urlStr = $"{APIBaseUrl}/hubs/{hubName}/streams/{ekey}/snapshot";
            HttpWebResponse response = null;
            try
            {
                var url = new Uri(urlStr);
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                var jsonobj = JsonConvert.SerializeObject(options);
                var body = Encoding.UTF8.GetBytes(jsonobj);
                var macToken = credentials.SignRequest(url, "POST", body, ContentType);
                request.ContentType = ContentType;
#if NET45
                request.UserAgent = Utils.UserAgent;
                request.Headers.Add("Authorization", macToken);
#else
                request.Headers["User-Agent"] = Utils.UserAgent;
                request.Headers["Authorization"] = macToken;
#endif

                using (var requestStream = request.GetRequestStreamAsync().Result)
                {
                    new MemoryStream(body).CopyTo(requestStream);
                }

                response = (HttpWebResponse)request.GetResponseAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                throw new PiliException(e);
            }

            // response never be null
            if ((int)response.StatusCode == 200)
            {
                try
                {
                    var reader = new StreamReader(response.GetResponseStream());
                    var text = reader.ReadToEnd();
                    return JObject.Parse(text);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.ToString());
                    Console.Write(e.StackTrace);
                    throw new PiliException(e);
                }
            }

            throw new PiliException(response);
        }

        public static void UpdateConverts(Credentials credentials, string hubName, string ekey, List<string> profiles)
        {
            var urlStr = $"{APIBaseUrl}/hubs/{hubName}/streams/{ekey}/converts";
            var json = new Dictionary<string, List<string>>
            {
                { "converts", profiles }
            };
            HttpWebResponse response = null;
            try
            {
                var url = new Uri(urlStr);
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                var jsonobj = JsonConvert.SerializeObject(json);
                var body = Encoding.UTF8.GetBytes(jsonobj);
                var macToken = credentials.SignRequest(url, "POST", body, ContentType);
                request.ContentType = ContentType;
#if NET45
                request.UserAgent = Utils.UserAgent;
                request.Headers.Add("Authorization", macToken);
#else
                request.Headers["User-Agent"] = Utils.UserAgent;
                request.Headers["Authorization"] = macToken;
#endif

                using (var requestStream = request.GetRequestStreamAsync().Result)
                {
                    new MemoryStream(body).CopyTo(requestStream);
                }

                response = (HttpWebResponse)request.GetResponseAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                throw new PiliException(e);
            }
        }

        public static JObject HistoryActivity(Credentials credentials, string hubName, string ekey, long start, long end)
        {
            var urlStr = $"{APIBaseUrl}/hubs/{hubName}/streams/{ekey}/historyactivity?start={start}&end={end}";
            HttpWebResponse response = null;
            try
            {
                var url = new Uri(urlStr);
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                var macToken = credentials.SignRequest(url, "GET", null, null);
#if NET45
                request.UserAgent = Utils.UserAgent;
                request.Headers.Add("Authorization", macToken);
#else
                request.Headers["User-Agent"] = Utils.UserAgent;
                request.Headers["Authorization"] = macToken;
#endif
                response = (HttpWebResponse)request.GetResponseAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                throw new PiliException(e);
            }

            // response never be null
            if ((int)response.StatusCode == 200)
            {
                try
                {
                    var reader = new StreamReader(response.GetResponseStream());
                    var text = reader.ReadToEnd();
                    return JObject.Parse(text);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.ToString());
                    Console.Write(e.StackTrace);
                    throw new PiliException(e);
                }
            }

            throw new PiliException(response);
        }

        public static JObject BatchLiveStatus(Credentials credentials, string hubName, List<string> streamkeys)
        {
            var urlStr = $"{APIBaseUrl}/hubs/{hubName}/livestreams";
            var json = new Dictionary<string, List<string>>
            {
                { "items", streamkeys }
            };
            HttpWebResponse response = null;
            try
            {
                var url = new Uri(urlStr);
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                var jsonobj = JsonConvert.SerializeObject(json);
                var body = Encoding.UTF8.GetBytes(jsonobj);
                var macToken = credentials.SignRequest(url, "POST", body, ContentType);
                request.ContentType = ContentType;
#if NET45
                request.UserAgent = Utils.UserAgent;
                request.Headers.Add("Authorization", macToken);
#else
                request.Headers["User-Agent"] = Utils.UserAgent;
                request.Headers["Authorization"] = macToken;
#endif

                using (var requestStream = request.GetRequestStreamAsync().Result)
                {
                    new MemoryStream(body).CopyTo(requestStream);
                }

                response = (HttpWebResponse)request.GetResponseAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                throw new PiliException(e);
            }

            // response never be null
            if ((int)response.StatusCode == 200)
            {
                try
                {
                    var reader = new StreamReader(response.GetResponseStream());
                    var text = reader.ReadToEnd();
                    return JObject.Parse(text);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.ToString());
                    Console.Write(e.StackTrace);
                    throw new PiliException(e);
                }
            }

            throw new PiliException(response);
        }

        public static JObject StreamList(Credentials credentials, string hubName, bool live, string prefix, int limit, string marker)
        {
            var urlStr = $"{APIBaseUrl}/hubs/{hubName}/streams?liveonly={live}&prefix={prefix}&limit={limit}&marker={marker}";
            HttpWebResponse response = null;
            try
            {
                var url = new Uri(urlStr);
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                var macToken = credentials.SignRequest(url, "GET", null, null);
#if NET45
                request.UserAgent = Utils.UserAgent;
                request.Headers.Add("Authorization", macToken);
#else
                request.Headers["User-Agent"] = Utils.UserAgent;
                request.Headers["Authorization"] = macToken;
#endif
                response = (HttpWebResponse)request.GetResponseAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                throw new PiliException(e);
            }

            // response never be null
            if ((int)response.StatusCode == 200)
            {
                try
                {
                    var reader = new StreamReader(response.GetResponseStream());
                    var text = reader.ReadToEnd();
                    return JObject.Parse(text);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.ToString());
                    Console.Write(e.StackTrace);
                    throw new PiliException(e);
                }
            }

            throw new PiliException(response);
        }


        // Delete stream
        public static string DeleteStream(Credentials credentials, string hubName, string streamKey)
        {
            if (streamKey == null)
            {
                throw new PiliException(MessageConfig.NullStreamIdExceptionMsg);
            }

            var urlStr = $"{(Configuration.Instance.UseHttps ? "https" : "http")}://{Configuration.Instance.API_HOST}/v1/streams/z1.{hubName}.{streamKey}";
            HttpWebResponse response = null;
            try
            {
                var url = new Uri(urlStr);
                var request = (HttpWebRequest)WebRequest.Create(url);
                var macToken = credentials.SignRequest(url, "DELETE", null, null);
                request.Method = "DELETE";
#if NET45
                request.UserAgent = Utils.UserAgent;
                request.Headers.Add("Authorization", macToken);
#else
                request.Headers["User-Agent"] = Utils.UserAgent;
                request.Headers["Authorization"] = macToken;
#endif
                Console.WriteLine(macToken);
                response = (HttpWebResponse)request.GetResponseAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                throw new PiliException(e);
            }

            // response never be null
            if ((int)response.StatusCode == 204)
            {
                var text = "No Content";
                return text;
            }

            throw new PiliException(response);
        }
    }
}
