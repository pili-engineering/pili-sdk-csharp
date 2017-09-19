using System;
using System.Collections.Generic;
using pili_sdk_csharp.pili;
using pili_sdk_csharp.pili_qiniu;
using Xunit;

namespace pili_sdk_csharp
{
    public class Example
    {
        // Replace with your keys here
        private const string AccessKey = "";

        private const string SecretKey = "";

        // Replace with your hub name
        private const string HubName = "";

        // Change API host as necessary
        //
        // pili.qiniuapi.com as default
        // pili-lte.qiniuapi.com is the latest RC version
        //
        // static {
        //     Configuration.getInstance().setAPIHost("pili.qiniuapi.com"); // default
        // }

        private void Check()
        {
            Assert.NotNull(AccessKey);
            Assert.NotEqual(AccessKey, "");
            Assert.NotNull(SecretKey);
            Assert.NotEqual(SecretKey, "");
            Assert.NotNull(HubName);
            Assert.NotEqual(HubName, "");
        }

        private Stream CreateStream(Hub hub, string streamKey)
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

        private Stream GetStream(Hub hub, string streamId)
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

        private void ListStreams(Hub hub, string prefix)
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

        private void ListLiveStreams(Hub hub, string prefix)
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

        private void BatchQueryLiveStreams(Hub hub, List<string> streamkeys)
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

        private void UpdateStreamConverts(Stream stream)
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

        private void DisableStream(Stream stream)
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

        private void EnableStream(Stream stream)
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

        private void GetLiveStatus(Stream stream)
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

        private void HistoryActivity(Stream stream)
        {
            try
            {
                var records = stream.HistoryActivity(0, 1515214403);
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

        private void SavePlayback(Stream stream)
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

        private void SaveSnapshot(Stream stream)
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


        [Fact]
        public void Test()
        {
            Check();
            var credentials = new Credentials(AccessKey, SecretKey);
            var hub = new Hub(credentials, HubName);

            const string keyA = "1234567";
            const string keyB = "SomeTestB";
            Console.WriteLine("获得不存在的流A:");
            GetStream(hub, keyA);

            Console.WriteLine("创建流:");
            CreateStream(hub, keyA);

            Console.WriteLine("获得流:");
            var stream = GetStream(hub, keyA);
            DeleteStream(stream);

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
            var url = PiliUrl.RTMPPublishURL(credentials, "pili-publish.mowa-cloud.com", HubName, keyA, 3600);
            Console.WriteLine(url);
            url = stream.RTMPPublishURL("pili-publish.mowa-cloud.com", 3600);
            Console.WriteLine(url);

            Console.WriteLine("RTMP 直播放址:");
            url = PiliUrl.RTMPPlayURL("pili-live-rtmp.mowa-cloud.com", HubName, keyA);
            Console.WriteLine(url);
            url = stream.RTMPPlayURL("pili-live-rtmp.mowa-cloud.com");
            Console.WriteLine(url);

            Console.WriteLine("HLS 直播地址:");
            url = PiliUrl.HLSPlayURL("pili-live-hls.mowa-cloud.com", HubName, keyA);
            Console.WriteLine(url);
            url = stream.HLSPlayURL("pili-live-hls.mowa-cloud.com");
            Console.WriteLine(url);

            Console.WriteLine("HDL 直播地址:");
            url = PiliUrl.HDLPlayURL("pili-live-hdl.mowa-cloud.com", HubName, keyA);
            Console.WriteLine(url);
            url = stream.HDLPlayURL("pili-live-hdl.mowa-cloud.com");
            Console.WriteLine(url);

            Console.WriteLine("截图直播地址:");
            url = PiliUrl.SnapshotPlayURL("pili-live-snapshot.mowa-cloud.com", HubName, keyA);
            Console.WriteLine(url);
            url = stream.SnapshotPlayURL("pili-live-snapshot.mowa-cloud.com");
            Console.WriteLine(url);
        }
    }
}
