using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pili_sdk_csharp.pili_common;
using pili_sdk_csharp.pili_qiniu;

namespace pili_sdk_csharp.pili
{
    public class API
    {
        private static readonly string API_BASE_URL = string.Format("{0}://{1}/{2}", Configuration.Instance.USE_HTTPS ? "https" : "http", Configuration.Instance.API_HOST,
            Configuration.Instance.API_VERSION);

        private static HttpWebRequest mOkHttpClient;


        public static Stream createStream(Credentials credentials, string hubName, string title, string publishKey, string publishSecurity)
        {
            //  System.out.println("createStream:" + API_BASE_URL);
            var urlStr = API_BASE_URL + "/streams";
            Console.WriteLine("API_BASE_URL---------" + API_BASE_URL);

            var json = new Dictionary<string, string>();
            json.Add("hub", hubName);
            if (Utils.isArgNotEmpty(title))
            {
                if (title.Length < Config.TITLE_MIN_LENGTH || title.Length > Config.TITLE_MAX_LENGTH)
                {
                    throw new PiliException(MessageConfig.ILLEGAL_TITLE_MSG);
                }
                json.Add("title", title);
            }
            if (Utils.isArgNotEmpty(publishKey))
            {
                json.Add("publishKey", publishKey);
            }
            if (Utils.isArgNotEmpty(publishSecurity))
            {
                json.Add("publishSecurity", publishSecurity);
            }
            HttpWebResponse response = null;
            try
            {
                var url = new Uri(urlStr);
                mOkHttpClient = (HttpWebRequest)WebRequest.Create(url);
                var contentType = "application/json";
                mOkHttpClient.Method = WebRequestMethods.Http.Post;
                var jsonobj = JsonConvert.SerializeObject(json);
                var body = jsonobj.GetBytes(Config.UTF8);
                var macToken = credentials.signRequest(url, "POST", body, contentType);
                mOkHttpClient.ContentType = contentType;
                mOkHttpClient.UserAgent = Utils.UserAgent;
                mOkHttpClient.Headers.Add("Authorization", macToken);
                mOkHttpClient.ContentLength = body.Length;
                using (var requestStream = mOkHttpClient.GetRequestStream())
                {
                    Utils.CopyN(requestStream, new MemoryStream(body), body.Length);
                }
                response = (HttpWebResponse)mOkHttpClient.GetResponse();
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
                    var jsonObj = JObject.Parse(text);
                    return new Stream(jsonObj, credentials);
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

        // Get an exist stream
        public static Stream getStream(Credentials credentials, string streamId)
        {
            if (streamId == null)
            {
                throw new PiliException(MessageConfig.NULL_STREAM_ID_EXCEPTION_MSG);
            }
            var urlStr = string.Format("{0}/streams/{1}", API_BASE_URL, streamId);
            HttpWebResponse response = null;
            try
            {
                var url = new Uri(urlStr);
                mOkHttpClient = (HttpWebRequest)WebRequest.Create(url);
                mOkHttpClient.Method = WebRequestMethods.Http.Get;
                var macToken = credentials.signRequest(url, "GET", null, null);
                mOkHttpClient.UserAgent = Utils.UserAgent;
                mOkHttpClient.Headers.Add("Authorization", macToken);
                response = (HttpWebResponse)mOkHttpClient.GetResponse();
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
                    var jsonObj = JObject.Parse(text);
                    return new Stream(jsonObj, credentials);
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

        // List stream
        public static Stream.StreamList listStreams(Credentials credentials, string hubName, string startMarker, long limitCount, string titlePrefix)
        {
            try
            {
                var encoding = Encoding.UTF8;

                hubName = HttpUtility.UrlEncode(hubName);
                if (Utils.isArgNotEmpty(startMarker))
                {
                    startMarker = HttpUtility.UrlEncode(startMarker);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                throw new PiliException(e);
            }
            var urlStr = string.Format("{0}/streams?hub={1}", API_BASE_URL, hubName);
            if (Utils.isArgNotEmpty(startMarker))
            {
                urlStr += "&marker=" + startMarker;
            }
            if (limitCount > 0)
            {
                urlStr += "&limit=" + limitCount;
            }
            if (Utils.isArgNotEmpty(titlePrefix))
            {
                urlStr += "&title=" + titlePrefix;
            }
            HttpWebResponse response = null;
            try
            {
                var url = new Uri(urlStr);
                var macToken = credentials.signRequest(url, "GET", null, null);
                mOkHttpClient = (HttpWebRequest)WebRequest.Create(url);
                mOkHttpClient.Method = WebRequestMethods.Http.Get;
                mOkHttpClient.UserAgent = Utils.UserAgent;
                mOkHttpClient.Headers.Add("Authorization", macToken);
                response = (HttpWebResponse)mOkHttpClient.GetResponse();
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
                    var jsonObj = JObject.Parse(text);
                    return new Stream.StreamList(jsonObj, credentials);
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

        // get stream status      
        public static Stream.Status getStreamStatus(Credentials credentials, string streamId)
        {
            if (streamId == null)
            {
                throw new PiliException(MessageConfig.NULL_STREAM_ID_EXCEPTION_MSG);
            }
            var urlStr = string.Format("{0}/streams/{1}/status", API_BASE_URL, streamId);
            HttpWebResponse response = null;
            try
            {
                var url = new Uri(urlStr);
                mOkHttpClient = (HttpWebRequest)WebRequest.Create(url);
                var macToken = credentials.signRequest(url, "GET", null, null);
                mOkHttpClient.Method = WebRequestMethods.Http.Get;
                mOkHttpClient.UserAgent = Utils.UserAgent;
                mOkHttpClient.Headers.Add("Authorization", macToken);
                response = (HttpWebResponse)mOkHttpClient.GetResponse();
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
                    var jsonObj = JObject.Parse(text);
                    return new Stream.Status(jsonObj);
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

        // Update an exist stream    
        public static Stream updateStream(Credentials credentials, string streamId, string publishKey, string publishSecurity, bool disabled)
        {
            var json = new JObject();

            if (streamId == null)
            {
                throw new PiliException(MessageConfig.NULL_STREAM_ID_EXCEPTION_MSG);
            }
            if (Utils.isArgNotEmpty(publishKey))
            {
                json.Add("publishKey", publishKey);
            }
            if (Utils.isArgNotEmpty(publishSecurity))
            {
                json.Add("publishSecurity", publishSecurity);
            }
            json.Add("disabled", disabled);


            var urlStr = string.Format("{0}/streams/{1}", API_BASE_URL, streamId);
            HttpWebResponse response = null;
            try
            {
                var url = new Uri(urlStr);
                var jsonobj = JsonConvert.SerializeObject(json);
                var body = jsonobj.GetBytes(Config.UTF8);
                mOkHttpClient = (HttpWebRequest)WebRequest.Create(url);
                var contentType = "application/json";
                var macToken = credentials.signRequest(url, "POST", body, contentType);
                mOkHttpClient.Method = WebRequestMethods.Http.Post;
                mOkHttpClient.ContentType = contentType;
                mOkHttpClient.UserAgent = Utils.UserAgent;
                mOkHttpClient.Headers.Add("Authorization", macToken);
                mOkHttpClient.ContentLength = body.Length;
                using (var requestStream = mOkHttpClient.GetRequestStream())
                {
                    Utils.CopyN(requestStream, new MemoryStream(body), body.Length);
                }

                response = (HttpWebResponse)mOkHttpClient.GetResponse();
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
                    var jsonObj = JObject.Parse(text);
                    return new Stream(jsonObj, credentials);
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
        public static string deleteStream(Credentials credentials, string streamId)
        {
            if (streamId == null)
            {
                throw new PiliException(MessageConfig.NULL_STREAM_ID_EXCEPTION_MSG);
            }

            var urlStr = string.Format("{0}/streams/{1}", API_BASE_URL, streamId);
            HttpWebResponse response = null;
            try
            {
                var url = new Uri(urlStr);
                mOkHttpClient = (HttpWebRequest)WebRequest.Create(url);
                var macToken = credentials.signRequest(url, "DELETE", null, null);
                mOkHttpClient.Method = "DELETE";
                mOkHttpClient.UserAgent = Utils.UserAgent;
                mOkHttpClient.Headers.Add("Authorization", macToken);
                Console.WriteLine(macToken);
                response = (HttpWebResponse)mOkHttpClient.GetResponse();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                throw new PiliException(e);
            }

            // response never be null
            if ((int)response.StatusCode / 100 == 2)
            {
                var text = "No Content";
                return text;
            }
            throw new PiliException(response);
        }


        public static Stream.SaveAsResponse saveAs(Credentials credentials, string streamId, string fileName, string format, long start, long end, string notifyUrl,
            string pipleline)
        {
            if (streamId == null)
            {
                throw new PiliException(MessageConfig.NULL_STREAM_ID_EXCEPTION_MSG);
            }

            if (!Utils.isArgNotEmpty(fileName))
            {
                throw new PiliException(MessageConfig.ILLEGAL_FILE_NAME_EXCEPTION_MSG);
            }


            if (start < 0 || end < 0 || start > end)
            {
                throw new PiliException(MessageConfig.ILLEGAL_TIME_MSG);
            }


            var urlStr = string.Format("{0}/streams/{1}/saveas", API_BASE_URL, streamId);
            HttpWebResponse response = null;
            var json = new JObject();
            json.Add("name", fileName);
            if (Utils.isArgNotEmpty(notifyUrl))
            {
                json.Add("notifyUrl", notifyUrl);
            }
            if (start != 0)
            {
                json.Add("start", start);
            }
            if (end != 0)
            {
                json.Add("end", end);
            }
            json.Add("format", format);
            if (pipleline != "")
            {
                json.Add("pipleline", pipleline);
            }


            try
            {
                var url = new Uri(urlStr);
                mOkHttpClient = (HttpWebRequest)WebRequest.Create(url);
                var contentType = "application/json";
                var body = json.ToString().GetBytes(Config.UTF8);
                var macToken = credentials.signRequest(url, "POST", body, contentType);
                mOkHttpClient.Method = WebRequestMethods.Http.Post;
                mOkHttpClient.ContentType = contentType;
                mOkHttpClient.UserAgent = Utils.UserAgent;
                mOkHttpClient.Headers.Add("Authorization", macToken);
                mOkHttpClient.ContentLength = body.Length;
                using (var requestStream = mOkHttpClient.GetRequestStream())
                {
                    Utils.CopyN(requestStream, new MemoryStream(body), body.Length);
                }
                response = (HttpWebResponse)mOkHttpClient.GetResponse();
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
                    var jsonObj = JObject.Parse(text);
                    return new Stream.SaveAsResponse(jsonObj);
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

        public static Stream.SnapshotResponse snapshot(Credentials credentials, string streamId, string fileName, string format, long time, string notifyUrl)
        {
            if (streamId == null)
            {
                throw new PiliException(MessageConfig.NULL_STREAM_ID_EXCEPTION_MSG);
            }

            if (!Utils.isArgNotEmpty(fileName))
            {
                throw new PiliException(MessageConfig.ILLEGAL_FILE_NAME_EXCEPTION_MSG);
            }

            if (!Utils.isArgNotEmpty(format))
            {
                throw new PiliException(MessageConfig.ILLEGAL_FORMAT_EXCEPTION_MSG);
            }

            var urlStr = string.Format("{0}/streams/{1}/snapshot", API_BASE_URL, streamId);
            HttpWebResponse response = null;
            var json = new JObject();
            json.Add("name", fileName);
            json.Add("format", format);
            if (time > 0)
            {
                json.Add("time", time);
            }
            if (Utils.isArgNotEmpty(notifyUrl))
            {
                json.Add("notifyUrl", notifyUrl); // optional
            }

            try
            {
                var url = new Uri(urlStr);
                mOkHttpClient = (HttpWebRequest)WebRequest.Create(url);
                var contentType = "application/json";
                var body = json.ToString().GetBytes(Config.UTF8);
                var macToken = credentials.signRequest(url, "POST", body, contentType);
                mOkHttpClient.Method = WebRequestMethods.Http.Post;
                mOkHttpClient.ContentType = contentType;
                mOkHttpClient.UserAgent = Utils.UserAgent;
                mOkHttpClient.Headers.Add("Authorization", macToken);
                mOkHttpClient.ContentLength = body.Length;
                using (var requestStream = mOkHttpClient.GetRequestStream())
                {
                    Utils.CopyN(requestStream, new MemoryStream(body), body.Length);
                }
                response = (HttpWebResponse)mOkHttpClient.GetResponse();
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
                    var jsonObj = JObject.Parse(text);
                    return new Stream.SnapshotResponse(jsonObj);
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

        // Get recording segments from an exist stream
        public static Stream.SegmentList getStreamSegments(Credentials credentials, string streamId, long startTime, long endTime, int limitCount)
        {
            if (streamId == null)
            {
                throw new PiliException(MessageConfig.NULL_STREAM_ID_EXCEPTION_MSG);
            }
            var urlStr = string.Format("{0}/streams/{1}/segments", API_BASE_URL, streamId);
            if (startTime > 0 && endTime > 0 && startTime < endTime)
            {
                urlStr += "?start=" + startTime + "&end=" + endTime;
            }
            if (limitCount > 0)
            {
                urlStr += "&limit=" + limitCount;
            }
            HttpWebResponse response = null;
            try
            {
                var url = new Uri(urlStr);
                var macToken = credentials.signRequest(url, "GET", null, null);
                mOkHttpClient = (HttpWebRequest)WebRequest.Create(url);
                mOkHttpClient.Method = WebRequestMethods.Http.Get;
                mOkHttpClient.UserAgent = Utils.UserAgent;
                mOkHttpClient.Headers.Add("Authorization", macToken);
                response = (HttpWebResponse)mOkHttpClient.GetResponse();
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
                    var jsonObj = JObject.Parse(text);
                    if (string.IsNullOrEmpty(jsonObj["segments"].ToString()))
                    {
                        throw new PiliException("Segments is null");
                    }
                    return new Stream.SegmentList(jsonObj);
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

        //Generate a RTMP publish URL
        public static string publishUrl(Stream stream, long nonce)
        {
            const string defaultScheme = "rtmp";
            if ("dynamic".Equals(stream.PublishSecurity))
            {
                return generateDynamicUrl(stream, nonce, defaultScheme);
            }
            if ("static".Equals(stream.PublishSecurity))
            {
                return generateStaticUrl(stream, defaultScheme);
            }
            // "dynamic" as default 
            return generateDynamicUrl(stream, nonce, defaultScheme);
        }

        //Generate RTMP live play URL
        public static IDictionary<string, string> rtmpLiveUrl(Stream stream)
        {
            const string defaultScheme = "rtmp";

            var url = string.Format("{0}://{1}/{2}/{3}", defaultScheme, stream.LiveRtmpHost, stream.HubName, stream.Title);
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary[Stream.ORIGIN] = url;
            var profiles = stream.Profiles;
            if (profiles != null)
            {
                foreach (var p in profiles)
                {
                    dictionary[p] = url + '@' + p;
                }
            }
            return dictionary;
        }

        //Generate HLS live play URL
        public static IDictionary<string, string> hlsLiveUrl(Stream stream)
        {
            const string defaultScheme = "http";
            var url = string.Format("{0}://{1}/{2}/{3}", defaultScheme, stream.LiveHttpHost, stream.HubName, stream.Title);
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary[Stream.ORIGIN] = url + ".m3u8";
            var profiles = stream.Profiles;
            if (profiles != null)
            {
                foreach (var p in profiles)
                {
                    dictionary[p] = url + '@' + p + ".m3u8";
                }
            }
            return dictionary;
        }

        //Generate HLS playback URL

        public static IDictionary<string, string> hlsPlaybackUrl(Credentials credentials, string streamId, long startTime, long endTime)
        {
            var saveFile = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds / 1000;

            var response = saveAs(credentials, streamId, saveFile.ToString(), null, startTime, endTime, null, null);

            IDictionary<string, string> dictionary = new Dictionary<string, string>();

            if (response != null)
            {
                dictionary.Add(Stream.ORIGIN, response.Url);
            }
            return dictionary;
        }

        public static IDictionary<string, string> httpFlvLiveUrl(Stream stream)
        {
            /* 
             * http://liveHttpFlvHost/hub/title@480p.flv
             */
            const string defaultScheme = "http";
            var url = string.Format("{0}://{1}/{2}/{3}", defaultScheme, stream.LiveHttpHost, stream.HubName, stream.Title);
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary[Stream.ORIGIN] = url + ".flv";
            var profiles = stream.Profiles;
            if (profiles != null)
            {
                foreach (var p in profiles)
                {
                    dictionary[p] = url + '@' + p + ".flv";
                }
            }
            return dictionary;
        }

        private static string generateStaticUrl(Stream stream, string scheme)
        {
            return string.Format("{0}://{1}/{2}/{3}?key={4}", scheme, stream.PublishRtmpHost, stream.HubName, stream.Title, stream.PublishKey);
        }

        private static string generateDynamicUrl(Stream stream, long nonce, string scheme)
        {
            if (nonce <= 0)
            {
                nonce = DateTimeHelperClass.CurrentUnixTimeMillis() / 1000; // the unit should be second
            }

            var baseUri = "/" + stream.HubName + "/" + stream.Title + "?nonce=" + nonce;
            string publishToken = null;
            try
            {
                publishToken = Credentials.sign(stream.PublishKey, baseUri);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                throw new PiliException(e);
            }
            return string.Format("{0}://{1}{2}&token={3}", scheme, stream.PublishRtmpHost, baseUri, publishToken);
        }
    }
}
