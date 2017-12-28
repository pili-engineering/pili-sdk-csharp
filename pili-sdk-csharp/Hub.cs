using System;
using Newtonsoft.Json;
using Qiniu.Pili.Hubs;
using Qiniu.Pili.Internal;
using Qiniu.Pili.Streams;

namespace Qiniu.Pili
{
    public sealed class Hub
    {
        private readonly string _baseUrl;
        private readonly RPC _cli;
        private readonly string _hub;

        internal Hub(RPC cli, string hub)
        {
            _cli = cli;
            _hub = hub;
            _baseUrl = $"{Config.APIHttpScheme}{Config.APIHost}/v2/hubs/{hub}";
        }

        /// <summary>
        ///     Create stream
        /// </summary>
        /// <exception cref="PiliException"></exception>
        public Stream Create(string streamKey)
        {
            var path = $"{_baseUrl}/streams";
            var args = new CreateArgs(streamKey);
            var json = JsonConvert.SerializeObject(args);

            try
            {
                _cli.CallWithJsonAsync(path, json).GetAwaiter().GetResult();
                var streamInfo = new StreamInfo(_hub, streamKey);
                return new Stream(streamInfo, _cli);
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
        ///     Get stream
        /// </summary>
        /// <exception cref="PiliException"></exception>
        public Stream Get(string streamKey)
        {
            try
            {
                var ekey = Base64UrlEncoder.Encode(streamKey);
                var path = $"{_baseUrl}/streams/{ekey}";

                var resp = _cli.CallWithGetAsync(path).Result;
                var ret = JsonConvert.DeserializeObject<StreamInfo>(resp);
                ret.SetMeta(_hub, streamKey);
                return new Stream(ret, _cli);
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

        private ListRet List(bool live, string prefix, int limit, string marker)
        {
            var path = $"{_baseUrl}/streams?liveonly={live}&prefix={prefix}&limit={limit:D}&marker={marker}";
            try
            {
                var resp = _cli.CallWithGetAsync(path).Result;

                var ret = JsonConvert.DeserializeObject<ApiRet>(resp);

                var listRet = new ListRet { Keys = new string[ret.Items.Length] };
                for (var i = 0; i < ret.Items.Length; i++)
                {
                    listRet.Keys[i] = ret.Items[i].Key;
                }

                listRet.Omarker = ret.Marker;
                return listRet;
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
        ///     List stream
        /// </summary>
        /// <exception cref="PiliException"></exception>
        public ListRet List(string prefix, int limit, string marker)
        {
            return List(false, prefix, limit, marker);
        }

        /// <summary>
        ///     List streams which is live
        /// </summary>
        /// <exception cref="PiliException"></exception>
        public ListRet ListLive(string prefix, int limit, string marker)
        {
            return List(true, prefix, limit, marker);
        }

        /// <summary>
        ///     Batch get live status
        /// </summary>
        /// <exception cref="PiliException"></exception>
        public BatchLiveStatus[] BatchGetLiveStatus(string[] streamTitles)
        {
            var path = _baseUrl + "/livestreams";
            var json = JsonConvert.SerializeObject(new BatchLiveStatusOptions(streamTitles));
            try
            {
                var resp = _cli.CallWithJsonAsync(path, json).Result;

                var ret = JsonConvert.DeserializeObject<BatchLiveStatusRet>(resp);
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
