using System;
using Newtonsoft.Json;
using Qiniu.Pili.Meetings;

namespace Qiniu.Pili
{
    public class Meeting
    {
        private readonly string _baseUrl;
        private readonly RPC _cli;

        internal Meeting(RPC cli)
        {
            _baseUrl = $"{Config.APIHttpScheme}{Config.RTCAPIHost}/v2";
            _cli = cli;
        }

        public virtual string CreateRoom(string ownerId, string roomName, int userMax)
        {
            var args = new CreateArgs(ownerId, roomName, userMax);
            return CreateRoom(args);
        }

        public virtual string CreateRoom(string ownerId, string roomName)
        {
            var args = new CreateArgs(ownerId, roomName);
            return CreateRoom(args);
        }

        public virtual string CreateRoom(string ownerId)
        {
            var args = new CreateArgs(ownerId);
            return CreateRoom(args);
        }

        private string CreateRoom(CreateArgs args)
        {
            var path = _baseUrl + "/rooms";
            var json = JsonConvert.SerializeObject(args);

            try
            {
                var resp = _cli.CallWithJsonAsync(path, json).Result;
                var ret = JsonConvert.DeserializeObject<RoomName>(resp);
                return ret.Name;
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

        public virtual Room GetRoom(string roomName)
        {
            var path = _baseUrl + "/rooms/" + roomName;
            try
            {
                var resp = _cli.CallWithGetAsync(path).Result;
                var room = JsonConvert.DeserializeObject<Room>(resp);
                return room;
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

        public virtual void DeleteRoom(string room)
        {
            var path = _baseUrl + "/rooms/" + room;
            try
            {
                _cli.CallWithDeleteAsync(path).GetAwaiter().GetResult();
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

        public virtual string RoomToken(string roomName, string userId, string perm, DateTime expireAt)
        {
            var access = new RoomAccess(roomName, userId, perm, expireAt);
            var json = JsonConvert.SerializeObject(access);
            return _cli.Mac.SignRoomToken(json);
        }
    }
}
