using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Qiniu.Pili.Internal;
using Qiniu.Pili.Streams;

namespace Qiniu.Pili
{
    public sealed class Stream
    {
        private readonly string _baseUrl;
        private readonly RPC _cli;
        private StreamInfo _info;

        internal Stream(StreamInfo info, RPC cli)
        {
            _info = info;
            var ekey = Base64UrlEncoder.Encode(info.Key);
            _baseUrl = $"{Config.APIHttpScheme}{Config.APIHost}/v2/hubs/{info.Hub}/streams/{ekey}";
            _cli = cli;
        }

        public string Hub => _info.Hub;

        public long DisabledTill
        {
            get => _info.DisabledTill;
            set
            {
                var args = new DisabledArgs { DisabledTill = value };
                var path = $"{_baseUrl}/disabled";
                var json = JsonConvert.SerializeObject(args);

                try
                {
                    _cli.CallWithJsonAsync(path, json).GetAwaiter().GetResult();
                }
                catch (PiliException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new PiliException(e);
                }
            }
        }

        public string[] Converts => _info.Converts;

        public string Key => _info.Key;

        /// <summary>
        ///     Fetch stream info
        /// </summary>
        /// <exception cref="PiliException"></exception>
        public Stream Info()
        {
            try
            {
                var resp = _cli.CallWithGetAsync(_baseUrl).GetAwaiter().GetResult();
                var ret = JsonConvert.DeserializeObject<StreamInfo>(resp);
                ret.SetMeta(_info.Hub, _info.Key);
                _info = ret;
                return this;
            }
            catch (PiliException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new PiliException(e);
            }
        }

        /// <summary>
        ///     Diable stream
        /// </summary>
        /// <exception cref="PiliException"></exception>
        public void Disable()
        {
            DisabledTill = -1;
        }

        /// <summary>
        ///     Disable stream
        /// </summary>
        /// <exception cref="PiliException"></exception>
        public void Disable(long disabledTill)
        {
            DisabledTill = disabledTill;
        }

        /// <summary>
        ///     Enable stream
        /// </summary>
        /// <exception cref="PiliException"></exception>
        public void Enable()
        {
            DisabledTill = 0;
        }

        /// <summary>
        ///     Get the status of live stream
        /// </summary>
        /// <exception cref="PiliException"></exception>
        public LiveStatus LiveStatus()
        {
            var path = _baseUrl + "/live";
            try
            {
                var resp = _cli.CallWithGetAsync(path).GetAwaiter().GetResult();
                var status = JsonConvert.DeserializeObject<LiveStatus>(resp);
                return status;
            }
            catch (PiliException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new PiliException(e);
            }
        }

        /// <summary>
        ///     Save playback
        /// </summary>
        /// <exception cref="PiliException"></exception>
        public string Save(long start, long end)
        {
            var args = new SaveOptions(start, end);
            return Save(args);
        }

        /// <summary>
        ///     Save playback with more options
        /// </summary>
        /// <exception cref="PiliException"></exception>
        public string Save(SaveOptions opts)
        {
            var path = _baseUrl + "/saveas";
            var json = JsonConvert.SerializeObject(opts);

            try
            {
                var resp = _cli.CallWithJsonAsync(path, json).GetAwaiter().GetResult();
                var ret = JsonConvert.DeserializeObject<SaveRet>(resp);
                return ret.Fname;
            }
            catch (PiliException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new PiliException(e);
            }
        }

        /// <exception cref="PiliException"></exception>
        public IDictionary<string, string> SaveReturn(SaveOptions opts)
        {
            var path = _baseUrl + "/saveas";
            var json = JsonConvert.SerializeObject(opts);

            try
            {
                var resp = _cli.CallWithJsonAsync(path, json).GetAwaiter().GetResult();
                var ret = JsonConvert.DeserializeObject<SaveRetFull>(resp);
                IDictionary<string, string> result = new Dictionary<string, string>
                {
                    ["persistentID"] = ret.PersistentId,
                    ["fname"] = ret.Fname
                };
                return result;
            }
            catch (PiliException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new PiliException(e);
            }
        }

        /// <summary>
        ///     Snapshot the live stream
        /// </summary>
        /// <exception cref="PiliException"></exception>
        public string Snapshot(SnapshotOptions opts)
        {
            var path = _baseUrl + "/snapshot";
            var json = JsonConvert.SerializeObject(opts);
            try
            {
                var resp = _cli.CallWithJsonAsync(path, json).GetAwaiter().GetResult();
                var ret = JsonConvert.DeserializeObject<SnapshotRet>(resp);
                return ret.Fname;
            }
            catch (PiliException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new PiliException(e);
            }
        }

        /// <summary>
        ///     Update convert configs
        /// </summary>
        /// <exception cref="PiliException"></exception>
        public void UpdateConverts(string[] profiles)
        {
            var path = _baseUrl + "/converts";
            var json = JsonConvert.SerializeObject(new ConvertsOptions(profiles));
            try
            {
                _cli.CallWithJsonAsync(path, json).GetAwaiter().GetResult();
            }
            catch (PiliException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new PiliException(e);
            }
        }

        /// <summary>
        ///     Query the stream history
        /// </summary>
        /// <exception cref="PiliException"></exception>
        public Record[] HistoryRecord(long start, long end)
        {
            string GenerateUrl()
            {
                var query = "";
                var flag = "?";

                if (start > 0)
                {
                    query += $"{flag}start={start:D}";
                    flag = "&";
                }

                if (end > 0)
                {
                    query += $"{flag}end={end:D}";
                }

                return $"{_baseUrl}/historyrecord{query}";
            }

            try
            {
                var resp = _cli.CallWithGetAsync(GenerateUrl()).GetAwaiter().GetResult();
                var ret = JsonConvert.DeserializeObject<HistoryRet>(resp);
                return ret.Items;
            }
            catch (PiliException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new PiliException(e);
            }
        }
    }
}
