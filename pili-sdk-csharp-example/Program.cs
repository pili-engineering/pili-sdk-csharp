using System;
using Qiniu.Pili;
using Qiniu.Pili.Meetings;
using Qiniu.Pili.Streams;

namespace pili_sdk_csharp_example
{
    public class Example
    {
        // Replace with your keys here ( https://portal.qiniu.com/user/key )
        private const string AccessKey = "";
        private const string SecretKey = "";

        // Replace with your hub name ( https://portal.qiniu.com/hub )
        private const string HubName = "";

        // Replace with your domain ( https://portal.qiniu.com/hub/{your_hub}/domain )
        private const string Domain = "example.com";

        private static void Main(string[] args)
        {
            var cli = new Client(AccessKey, SecretKey);
            var hub = cli.NewHub(HubName);
            const string prefix = "SomeTest";
            const string keyA = prefix + "A";
            const string keyB = prefix + "B";
            const string roomName = "test12Room";
            var meeting = cli.NewMeeting();
            Console.WriteLine("获得不存在的流A:");
            try
            {
                hub.Get(keyA);
            }
            catch (PiliException e)
            {
                if (e.NotFound)
                {
                    Console.WriteLine($"Stream {keyA} doesn't exist");
                }
                else
                {
                    Console.WriteLine($"{keyA} should not exist");
                    Console.WriteLine(e.StackTrace);
                    throw;
                }
            }

            Console.WriteLine($"keyA={keyA} 不存在");

            Console.WriteLine("创建流:");
            try
            {
                hub.Create(keyA);
            }
            catch (PiliException e)
            {
                Console.WriteLine(e.StackTrace);
                throw;
            }

            Console.WriteLine($"KeyA={keyA} 创建");

            Stream streamA;
            Console.WriteLine("获得流:");
            try
            {
                streamA = hub.Get(keyA);
            }
            catch (PiliException e)
            {
                Console.WriteLine(e.StackTrace);
                throw;
            }

            Console.WriteLine($"keyA={keyA} 查询: {streamA}");

            Console.WriteLine("创建重复流:");
            try
            {
                hub.Create(keyA);
            }
            catch (PiliException e)
            {
                if (!e.Duplicate)
                {
                    Console.WriteLine(e.StackTrace);
                    throw;
                }
            }

            Console.WriteLine($"keyA=%{keyA} 已存在");

            Stream streamB;
            Console.WriteLine("创建另一路流:");
            try
            {
                streamB = hub.Create(keyB);
            }
            catch (PiliException e)
            {
                Console.WriteLine(e.StackTrace);
                throw;
            }

            Console.WriteLine($"keyB={keyB} 创建: {streamB}");

            Console.WriteLine("列出流:");
            try
            {
                var listRet = hub.List(prefix, 0, "");
                var list = listRet.Keys;
                foreach (var s in list)
                {
                    Console.WriteLine(s);
                }

                Console.WriteLine($"marker: {listRet.Omarker}");
            }
            catch (PiliException e)
            {
                Console.WriteLine(e.StackTrace);
                throw;
            }


            Console.WriteLine("列出正在直播的流:");
            try
            {
                var listRet = hub.ListLive(prefix, 0, "");
                var list = listRet.Keys;
                foreach (var s in list)
                {
                    Console.WriteLine(s);
                }

                Console.WriteLine($"marker: {listRet.Omarker}");
            }
            catch (PiliException e)
            {
                Console.WriteLine(e.StackTrace);
                throw;
            }

            Console.WriteLine("禁用流:");
            try
            {
                streamA.Enable();
                streamA = hub.Get(keyA);
            }
            catch (PiliException e)
            {
                Console.WriteLine(e.StackTrace);
                throw;
            }

            Console.WriteLine($"keyA={keyA} 启用: {streamA}");

            Console.WriteLine("启用流:");
            try
            {
                streamA.Enable();
                streamA = hub.Get(keyA);
            }
            catch (PiliException e)
            {
                Console.WriteLine(e.StackTrace);
                throw;
            }

            Console.WriteLine($"keyA={keyA} 启用: {streamA}");

            Console.WriteLine("查询直播状态:");
            try
            {
                var status = streamA.LiveStatus();
                Console.WriteLine($"keyA={keyA} 直播状态:status={status}");
            }
            catch (PiliException e)
            {
                if (!e.NotInLive)
                {
                    Console.WriteLine(e.StackTrace);
                    throw;
                }

                Console.WriteLine($"keyA={keyA} 不在直播", keyA);
            }

            Console.WriteLine("更改流的实时转码规格:");
            try

            {
                streamA.UpdateConverts(new[] { "480p", "720p" });
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }


            Console.WriteLine("查询推流历史:");
            try
            {
                var records = streamA.HistoryRecord(0, 0);
                foreach (var record in records)
                {
                    Console.WriteLine($"Record start={record.Start}, end={record.End}");
                }
            }
            catch (PiliException e)
            {
                Console.WriteLine(e.StackTrace);
                throw;
            }

            Console.WriteLine("保存直播数据:");
            try
            {
                var fName = streamA.Save(0, 0);
                Console.WriteLine($"keyA={keyA} 保存直播数据: fname={fName}\n");
            }
            catch (PiliException e)
            {
                if (!e.NotInLive)
                {
                    Console.WriteLine(e.StackTrace);
                    throw;
                }

                Console.WriteLine($"keyA={keyA} 不在直播\n");
            }

            Console.WriteLine("保存直播数据并获取作业id:");
            try
            {
                var options = new SaveOptions
                {
                    Start = 0,
                    End = 0,
                    Format = "mp4"
                };

                var ret = streamA.SaveReturn(options);
                ret.TryGetValue("fname", out var fName);
                Console.WriteLine("fname:" + fName);
                ret.TryGetValue("fname", out var persistentId);
                Console.WriteLine("persistentID:" + persistentId);
            }
            catch (PiliException e)
            {
                if (!e.NotInLive)
                {
                    Console.WriteLine(e.StackTrace);
                    throw;
                }

                Console.WriteLine($"keyA={keyA} 不在直播\n");
            }

            Console.WriteLine("RTMP 推流地址:");
            var url = cli.RTMPPublishURL("pili-publish." + Domain, HubName, keyA, 3600);
            Console.WriteLine($"keyA={keyA} RTMP推流地址={url}");

            Console.WriteLine("RTMP 直播放址:");
            url = cli.RTMPPlayURL("pili-live-rtmp." + Domain, HubName, keyA);
            Console.WriteLine($"keyA={keyA} RTMP直播地址={url}");

            Console.WriteLine("HLS 直播地址:");
            url = cli.HLSPlayURL("pili-live-hls." + Domain, HubName, keyA);
            Console.WriteLine($"keyA={keyA} HLS直播地址={url}");

            Console.WriteLine("HDL 直播地址:");
            url = cli.HDLPlayURL("pili-live-hls." + Domain, HubName, keyA);
            Console.WriteLine($"keyA={keyA} HDL直播地址={url}");

            Console.WriteLine("截图直播地址:");
            url = cli.SnapshotPlayURL("pili-live-smapshot." + Domain, HubName, keyA);
            Console.WriteLine($"keyA={keyA} 截图直播地址={url}");

            Console.WriteLine("创建房间:");
            var r1 = meeting.CreateRoom("123", roomName, 12);
            Console.WriteLine($"Expect RoomName: {roomName}, Actual: {r1}");
            var room = meeting.GetRoom(roomName);
            Console.WriteLine("roomName:" + room.Name);
            Console.WriteLine("roomStatus:" + room.Status);
            Console.WriteLine($"Expect RoomName: {roomName}, Actual: {room.Name}");
            Console.WriteLine($"Expect OwnerId: admin, Actual: {room.OwnerId}");
            Console.WriteLine($"Expect Status: {Status.New}, Actual: {room.Status}");

            var token = meeting.RoomToken("room1", "123", "admin", new DateTime(1785600000000L));
            Console.WriteLine($"Token:{token}");

            Console.WriteLine("删除房间:");
            meeting.DeleteRoom(roomName);

            Console.ReadKey();
        }
    }
}
