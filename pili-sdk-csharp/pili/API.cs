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

        public static Stream CreateStream(Credentials credentials, string hubName, string title, string publishKey, string publishSecurity)
        {
            //  System.out.println("createStream:" + API_BASE_URL);
            var urlStr = APIBaseUrl + "/streams";
            Console.WriteLine("API_BASE_URL---------" + APIBaseUrl);

            var json = new Dictionary<string, string>
            {
                { "hub", hubName }
            };
            if (!string.IsNullOrWhiteSpace(title))
            {
                if (title.Length < Config.TitleMinLength || title.Length > Config.TitleMaxLength)
                {
                    throw new PiliException(MessageConfig.IllegalTitleMsg);
                }
                json.Add("title", title);
            }
            if (!string.IsNullOrWhiteSpace(publishKey))
            {
                json.Add("publishKey", publishKey);
            }
            if (!string.IsNullOrWhiteSpace(publishSecurity))
            {
                json.Add("publishSecurity", publishSecurity);
            }
            HttpWebResponse response;
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
        public static Stream GetStream(Credentials credentials, string streamId)
        {
            if (streamId == null)
            {
                throw new PiliException(MessageConfig.NullStreamIdExceptionMsg);
            }
            var urlStr = $"{APIBaseUrl}/streams/{streamId}";
            HttpWebResponse response;
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
        public static Stream.StreamList ListStreams(Credentials credentials, string hubName, string startMarker, long limitCount, string titlePrefix)
        {
            try
            {
                hubName = WebUtility.UrlEncode(hubName);
                if (!string.IsNullOrWhiteSpace(startMarker))
                {
                    startMarker = WebUtility.UrlEncode(startMarker);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                throw new PiliException(e);
            }
            var urlStr = $"{APIBaseUrl}/streams?hub={hubName}";
            if (!string.IsNullOrWhiteSpace(startMarker))
            {
                urlStr += "&marker=" + startMarker;
            }
            if (limitCount > 0)
            {
                urlStr += "&limit=" + limitCount;
            }
            if (!string.IsNullOrWhiteSpace(titlePrefix))
            {
                urlStr += "&title=" + titlePrefix;
            }
            HttpWebResponse response;
            try
            {
                var url = new Uri(urlStr);
                var macToken = credentials.SignRequest(url, "GET", null, null);
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
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
        public static Stream.StreamStatus GetStreamStatus(Credentials credentials, string streamId)
        {
            if (streamId == null)
            {
                throw new PiliException(MessageConfig.NullStreamIdExceptionMsg);
            }
            var urlStr = $"{APIBaseUrl}/streams/{streamId}/status";
            HttpWebResponse response;
            try
            {
                var url = new Uri(urlStr);
                var request = (HttpWebRequest)WebRequest.Create(url);
                var macToken = credentials.SignRequest(url, "GET", null, null);
                request.Method = "GET";
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
                    var jsonObj = JObject.Parse(text);
                    return new Stream.StreamStatus(jsonObj);
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
        public static Stream UpdateStream(Credentials credentials, string streamId, string publishKey, string publishSecurity, bool disabled)
        {
            var json = new JObject();

            if (streamId == null)
            {
                throw new PiliException(MessageConfig.NullStreamIdExceptionMsg);
            }
            if (!string.IsNullOrWhiteSpace(publishKey))
            {
                json.Add("publishKey", publishKey);
            }
            if (!string.IsNullOrWhiteSpace(publishSecurity))
            {
                json.Add("publishSecurity", publishSecurity);
            }
            json.Add("disabled", disabled);

            var urlStr = $"{APIBaseUrl}/streams/{streamId}";
            HttpWebResponse response;
            try
            {
                var url = new Uri(urlStr);
                var jsonobj = JsonConvert.SerializeObject(json);
                var body = Encoding.UTF8.GetBytes(jsonobj);
                var request = (HttpWebRequest)WebRequest.Create(url);
                var macToken = credentials.SignRequest(url, "POST", body, ContentType);
                request.Method = "POST";
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
        public static string DeleteStream(Credentials credentials, string streamId)
        {
            if (streamId == null)
            {
                throw new PiliException(MessageConfig.NullStreamIdExceptionMsg);
            }

            var urlStr = $"{APIBaseUrl}/streams/{streamId}";
            HttpWebResponse response;
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
            if ((int)response.StatusCode / 100 == 2)
            {
                var text = "No Content";
                return text;
            }
            throw new PiliException(response);
        }


        public static Stream.SaveAsResponse SaveAs(Credentials credentials, string streamId, string fileName, string format, long start, long end, string notifyUrl,
            string pipleline)
        {
            if (streamId == null)
            {
                throw new PiliException(MessageConfig.NullStreamIdExceptionMsg);
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new PiliException(MessageConfig.IllegalFileNameExceptionMsg);
            }

            if (start < 0 || end < 0 || start > end)
            {
                throw new PiliException(MessageConfig.IllegalTimeMsg);
            }

            var urlStr = $"{APIBaseUrl}/streams/{streamId}/saveas";
            HttpWebResponse response;
            var json = new JObject
            {
                { "name", fileName }
            };
            if (!string.IsNullOrWhiteSpace(notifyUrl))
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
                var request = (HttpWebRequest)WebRequest.Create(url);
                var body = Encoding.UTF8.GetBytes(json.ToString());
                var macToken = credentials.SignRequest(url, "POST", body, ContentType);
                request.Method = "POST";
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

        public static Stream.SnapshotResponse Snapshot(Credentials credentials, string streamId, string fileName, string format, long time, string notifyUrl)
        {
            if (streamId == null)
            {
                throw new PiliException(MessageConfig.NullStreamIdExceptionMsg);
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new PiliException(MessageConfig.IllegalFileNameExceptionMsg);
            }

            if (string.IsNullOrWhiteSpace(format))
            {
                throw new PiliException(MessageConfig.IllegalFormatExceptionMsg);
            }

            var urlStr = $"{APIBaseUrl}/streams/{streamId}/snapshot";
            HttpWebResponse response;
            var json = new JObject
            {
                { "name", fileName },
                { "format", format }
            };
            if (time > 0)
            {
                json.Add("time", time);
            }
            if (!string.IsNullOrWhiteSpace(notifyUrl))
            {
                json.Add("notifyUrl", notifyUrl); // optional
            }

            try
            {
                var url = new Uri(urlStr);
                var request = (HttpWebRequest)WebRequest.Create(url);
                var body = Encoding.UTF8.GetBytes(json.ToString());
                var macToken = credentials.SignRequest(url, "POST", body, ContentType);
                request.Method = "POST";
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
        public static Stream.SegmentList GetStreamSegments(Credentials credentials, string streamId, long startTime, long endTime, int limitCount)
        {
            if (streamId == null)
            {
                throw new PiliException(MessageConfig.NullStreamIdExceptionMsg);
            }
            var urlStr = $"{APIBaseUrl}/streams/{streamId}/segments";
            if (startTime > 0 && endTime > 0 && startTime < endTime)
            {
                urlStr += "?start=" + startTime + "&end=" + endTime;
            }
            if (limitCount > 0)
            {
                urlStr += "&limit=" + limitCount;
            }
            HttpWebResponse response;
            try
            {
                var url = new Uri(urlStr);
                var macToken = credentials.SignRequest(url, "GET", null, null);
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
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
        public static string PublishUrl(Stream stream, long nonce)
        {
            const string defaultScheme = "rtmp";
            if ("dynamic".Equals(stream.PublishSecurity))
            {
                return GenerateDynamicUrl(stream, nonce, defaultScheme);
            }
            if ("static".Equals(stream.PublishSecurity))
            {
                return GenerateStaticUrl(stream, defaultScheme);
            }
            // "dynamic" as default 
            return GenerateDynamicUrl(stream, nonce, defaultScheme);
        }

        //Generate RTMP live play URL
        public static IDictionary<string, string> RtmpLiveUrl(Stream stream)
        {
            const string defaultScheme = "rtmp";

            var url = $"{defaultScheme}://{stream.LiveRtmpHost}/{stream.HubName}/{stream.Title}";
            IDictionary<string, string> dictionary = new Dictionary<string, string>
            {
                [Stream.Origin] = url
            };
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
        public static IDictionary<string, string> HlsLiveUrl(Stream stream)
        {
            const string defaultScheme = "http";
            var url = $"{defaultScheme}://{stream.LiveHttpHost}/{stream.HubName}/{stream.Title}";
            IDictionary<string, string> dictionary = new Dictionary<string, string>
            {
                [Stream.Origin] = url + ".m3u8"
            };
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

        public static IDictionary<string, string> HlsPlaybackUrl(Credentials credentials, string streamId, long startTime, long endTime)
        {
            var saveFile = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds / 1000;

            var response = SaveAs(credentials, streamId, saveFile.ToString(), null, startTime, endTime, null, null);

            IDictionary<string, string> dictionary = new Dictionary<string, string>();

            if (response != null)
            {
                dictionary.Add(Stream.Origin, response.Url);
            }
            return dictionary;
        }

        public static IDictionary<string, string> HttpFlvLiveUrl(Stream stream)
        {
            /* 
             * http://liveHttpFlvHost/hub/title@480p.flv
             */
            const string defaultScheme = "http";
            var url = $"{defaultScheme}://{stream.LiveHttpHost}/{stream.HubName}/{stream.Title}";
            IDictionary<string, string> dictionary = new Dictionary<string, string>
            {
                [Stream.Origin] = url + ".flv"
            };
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

        private static string GenerateStaticUrl(Stream stream, string scheme)
        {
            return $"{scheme}://{stream.PublishRtmpHost}/{stream.HubName}/{stream.Title}?key={stream.PublishKey}";
        }

        private static string GenerateDynamicUrl(Stream stream, long nonce, string scheme)
        {
            if (nonce <= 0)
            {
                nonce = DateTimeHelper.CurrentUnixTimeSeconds();
            }

            var baseUri = "/" + stream.HubName + "/" + stream.Title + "?nonce=" + nonce;
            string publishToken;
            try
            {
                publishToken = Credentials.Sign(stream.PublishKey, baseUri);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                throw new PiliException(e);
            }
            return $"{scheme}://{stream.PublishRtmpHost}{baseUri}&token={publishToken}";
        }
    }
}
