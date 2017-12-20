using System;
using System.Collections.Generic;
using pili_sdk_csharp.pili;
using pili_sdk_csharp.pili_qiniu;

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
            var credentials = new Credentials(AccessKey, SecretKey);
            var hub = new Hub(credentials, HubName);

            const string keyA = "SomeTestA";
            const string keyB = "SomeTestB";

            Console.WriteLine("获得不存在的流A:");
            GetStream(hub, keyA);

            Console.WriteLine("创建流:");
            CreateStream(hub, keyA);

            Console.WriteLine("获得流:");
            var stream = GetStream(hub, keyA);

            Console.WriteLine("创建重复流:");
            CreateStream(hub, keyA);

            Console.WriteLine("创建另一路流:");
            CreateStream(hub, keyB);

            Console.WriteLine("列出流:");
            ListStreams(hub, "");

            Console.WriteLine("列出正在直播的流:");
            ListLiveStreams(hub, "");

            Console.WriteLine("批量查询直播信息:");
            BatchQueryLiveStreams(hub, new List<string> { keyA, keyB });

            Console.WriteLine("更改流的实时转码规格:");
            UpdateStreamConverts(stream);

            Console.WriteLine("禁用流:");
            DisableStream(stream);

            Console.WriteLine("启用流:");
            EnableStream(stream);

            Console.WriteLine("查询直播状态:");
            GetLiveStatus(stream);

            Console.WriteLine("查询推流历史:");
            HistoryActivity(stream);

            Console.WriteLine("保存直播数据:");
            SavePlayback(stream);

            Console.WriteLine("保存直播截图:");
            SaveSnapshot(stream);

            Console.WriteLine("RTMP 推流地址:");
            var url = PiliUrl.RTMPPublishURL(credentials, $"pili-publish.{Domain}", HubName, keyA, 3600);
            Console.WriteLine(url);
            url = stream.RTMPPublishURL($"pili-publish.{Domain}", 3600);
            Console.WriteLine(url);

            Console.WriteLine("RTMP 直播放址:");
            url = PiliUrl.RTMPPlayURL($"pili-live-rtmp.{Domain}", HubName, keyA);
            Console.WriteLine(url);
            url = stream.RTMPPlayURL($"pili-live-rtmp.{Domain}");
            Console.WriteLine(url);

            Console.WriteLine("HLS 直播地址:");
            url = PiliUrl.HLSPlayURL($"pili-live-hls.{Domain}", HubName, keyA);
            Console.WriteLine(url);
            url = stream.HLSPlayURL($"pili-live-hls.{Domain}");
            Console.WriteLine(url);

            Console.WriteLine("HDL 直播地址:");
            url = PiliUrl.HDLPlayURL($"pili-live-hdl.{Domain}", HubName, keyA);
            Console.WriteLine(url);
            url = stream.HDLPlayURL($"pili-live-hdl.{Domain}");
            Console.WriteLine(url);

            Console.WriteLine("截图直播地址:");
            url = PiliUrl.SnapshotPlayURL($"pili-live-snapshot.{Domain}", HubName, keyA);
            Console.WriteLine(url);
            url = stream.SnapshotPlayURL($"pili-live-snapshot.{Domain}");
            Console.WriteLine(url);
            Console.ReadKey();
        }

        private static Stream CreateStream(Hub hub, string streamKey)
        {
            try
            {
                return hub.CreateStream(streamKey);
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }

            return null;
        }

        private static Stream GetStream(Hub hub, string streamId)
        {
            try
            {
                return hub.GetStream(streamId);
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }

            return null;
        }

        private Stream.StreamInfo StreamInfo(Stream stream)
        {
            try
            {
                return stream.Info();
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }

            return null;
        }

        private static void ListStreams(Hub hub, string prefix)
        {
            try
            {
                var streamList = hub.List(prefix, 10, "");
                Console.WriteLine("marker:" + streamList.Marker);
                var list = streamList.Keys;
                foreach (var s in list)
                {
                    Console.WriteLine(s);
                }
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }

        private static void ListLiveStreams(Hub hub, string prefix)
        {
            try
            {
                var streamList = hub.ListLive(prefix, 10, "");
                Console.WriteLine("marker:" + streamList.Marker);
                var list = streamList.Keys;
                foreach (var s in list)
                {
                    Console.WriteLine(s);
                }
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }

        private static void BatchQueryLiveStreams(Hub hub, List<string> streamkeys)
        {
            try
            {
                var liveStatus = hub.BatchLiveStatus(streamkeys);
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }

        private static void UpdateStreamConverts(Stream stream)
        {
            try
            {
                stream.UpdateConverts(new List<string> { "480p", "720p" });
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }

        private static void DisableStream(Stream stream)
        {
            try
            {
                stream.Disable();
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }

        private static void EnableStream(Stream stream)
        {
            try
            {
                stream.Enable();
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }

        private static void GetLiveStatus(Stream stream)
        {
            try
            {
                var status = stream.LiveStatus();
                Console.WriteLine(status.ToString());
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }

        private static void HistoryActivity(Stream stream)
        {
            try
            {
                var records = stream.HistoryActivity(0, 0);
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }

        private void DeleteStream(Stream stream)
        {
            try
            {
                stream.Delete();
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }

        private static void SavePlayback(Stream stream)
        {
            try
            {
                var response = stream.SaveAs(new Stream.SaveasOptions { Format = "mp4" });
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }

        private static void SaveSnapshot(Stream stream)
        {
            try
            {
                var response = stream.Snapshot(new Stream.SnapshotOptions { Format = "jpg" });
                Console.WriteLine(response);
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }
    }
}
