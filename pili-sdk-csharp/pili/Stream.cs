using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pili_sdk_csharp.pili_common;
using pili_sdk_csharp.pili_qiniu;

namespace pili_sdk_csharp.pili
{
    public class Stream
    {
        public const string Origin = "ORIGIN";
        private readonly string _ekey;
        private readonly string _hubName;
        private readonly string _key;
        private readonly Credentials _mCredentials;

        public Stream(string hubName, string key, Credentials credentials)
        {
            _hubName = hubName;
            _key = key;
            _ekey = UrlSafeBase64.Base64UrlEncode(Encoding.UTF8.GetBytes(key));
            _mCredentials = credentials;
        }

        public StreamInfo Info()
        {
            var info = API.GetStream(_mCredentials, _hubName, _ekey);
            return new StreamInfo(_hubName, _key, info);
        }

        public void Disable()
        {
            SetDisableTill(-1);
        }

        public void Enable()
        {
            SetDisableTill(0);
        }

        public void DisableTill(long till)
        {
            SetDisableTill(till);
        }

        public void SetDisableTill(long till)
        {
            API.SetDisableTill(_mCredentials, _hubName, _ekey, till);
        }

        public StreamStatus LiveStatus()
        {
            return new StreamStatus(API.LiveStatus(_mCredentials, _hubName, _ekey));
        }

        public SaveasResponse Save(long start, long end)
        {
            var options = new SaveasOptions(start, end);
            return new SaveasResponse(API.SaveAs(_mCredentials, _hubName, _ekey, options));
        }

        public SaveasResponse SaveAs(SaveasOptions options)
        {
            return new SaveasResponse(API.SaveAs(_mCredentials, _hubName, _ekey, options));
        }

        public string Snapshot(SnapshotOptions options)
        {
            return API.Snapshot(_mCredentials, _hubName, _ekey, options)["fname"].ToString();
        }

        public void UpdateConverts(List<string> profiles)
        {
            API.UpdateConverts(_mCredentials, _hubName, _ekey, profiles);
        }

        public List<Record> HistoryActivity(long start, long end)
        {
            return API.HistoryActivity(_mCredentials, _hubName, _ekey, start, end)["items"].ToObject<List<Record>>();
        }

        public void Delete()
        {
            API.DeleteStream(_mCredentials, _hubName, _key);
        }

        public string RTMPPublishURL(string domain, long expireAfterSeconds)
        {
            var expire = DateTimeHelper.CurrentUnixTimeSeconds() + expireAfterSeconds;
            var token = _mCredentials.SignStream(_hubName, _key, expire);
            const string defaultScheme = "rtmp";
            return $"{defaultScheme}://{domain}/{_hubName}/{_key}?e={expire}&token={token}";
        }

        public string RTMPPlayURL(string domain)
        {
            return $"rtmp://{domain}/{_hubName}/{_key}";
        }

        public string HLSPlayURL(string domain)
        {
            return $"http://{domain}/{_hubName}/{_key}.m3u8";
        }

        public string HDLPlayURL(string domain)
        {
            return $"http://{domain}/{_hubName}/{_key}.flv";
        }

        public string SnapshotPlayURL(string domain)
        {
            return $"http://{domain}/{_hubName}/{_key}.jpg";
        }

        public sealed class StreamInfo
        {
            public StreamInfo(string hubName, string key, JObject info)
            {
                HubName = hubName;
                Key = key;
                Converts = JsonConvert.DeserializeObject<List<string>>(info["converts"].ToString());
                CreatedAt = Convert.ToInt64(info["createdAt"].ToString());
                ExpireAt = Convert.ToInt64(info["expireAt"].ToString());
                UpdatedAt = Convert.ToInt64(info["updatedAt"].ToString());
                DisabledTill = Convert.ToInt64(info["disabledTill"].ToString());
                WaterMark = (bool)info["watermark"];
            }

            public string HubName { get; }
            public string Key { get; }
            public List<string> Converts { get; set; }
            public long CreatedAt { get; }
            public long ExpireAt { get; set; }
            public long UpdatedAt { get; }
            public long DisabledTill { get; set; }
            public bool WaterMark { get; set; }

            public bool Disabled()
            {
                return DisabledTill == -1 || DisabledTill > DateTimeHelper.CurrentUnixTimeSeconds();
            }
        }

        public class FPSStatus
        {
            public FPSStatus(float audio, float video, float data)
            {
                Audio = audio;
                Video = video;
                Data = data;
            }

            public virtual float Audio { get; }

            public virtual float Video { get; }

            public virtual float Data { get; }
        }

        public class StreamStatus
        {
            private readonly string _mJsonString;
            private readonly string _status;

            public StreamStatus(JObject jsonObj)
            {
                ClientIP = jsonObj["addr"].ToString();
                _status = jsonObj["status"].ToString();
                var startFrominit = (DateTime)jsonObj["startFrom"];
                StartAt = DateTimeHelper.TransUnixTimeSeconds(startFrominit);
                try
                {
                    Bps = (float)jsonObj["bytesPerSecond"];
                    var audio = (float)jsonObj["framesPerSecond"]["audio"];
                    var video = (float)jsonObj["framesPerSecond"]["video"];
                    var data = (float)jsonObj["framesPerSecond"]["data"];
                    Fps = new FPSStatus(audio, video, data);
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e.ToString());
                    Console.Write(e.StackTrace);
                }

                _mJsonString = jsonObj.ToString();
            }

            public virtual string ClientIP { get; }

            public virtual long StartAt { get; }

            public virtual float Bps { get; }

            public virtual FPSStatus Fps { get; }

            public virtual string GetStatus()
            {
                return _status;
            }

            public override string ToString()
            {
                return _mJsonString;
            }
        }

        public sealed class SaveasOptions
        {
            public SaveasOptions()
            {
            }

            public SaveasOptions(long start, long end)
            {
                Start = start;
                End = end;
            }

            public string Fname { get; set; }
            public long Start { get; set; }
            public long End { get; set; }
            public string Format { get; set; }
            public string Pipeline { get; set; }

            public string Notify { get; set; }

            // 对应ts文件的过期时间.
            // -1 表示不修改ts文件的expire属性.
            // 0  表示修改ts文件生命周期为永久保存.
            // >0 表示修改ts文件的的生命周期为ExpireDays.
            public long ExpireDays { get; set; }
        }

        public sealed class SaveasResponse
        {
            public SaveasResponse(JObject jsonObj)
            {
                Fname = jsonObj["fname"].ToString();
                PersistentId = jsonObj["persistentID"].ToString();
            }

            public string Fname { get; set; }
            public string PersistentId { get; set; }
        }

        public class SnapshotOptions
        {
            public virtual string Fname { get; set; }
            public virtual long Time { get; set; }
            public virtual string Format { get; set; }
        }

        public class Record
        {
            public virtual long Start { get; set; }
            public virtual long End { get; set; }
        }
    }
}
