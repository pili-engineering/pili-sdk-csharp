using Qiniu.Pili;
using Xunit;

namespace pili_sdk_csharp
{
    public class APITest
    {
        // Replace with your keys here ( https://portal.qiniu.com/user/key )
        private const string AccessKey = "";
        private const string SecretKey = "";

        // Replace with your hub name ( https://portal.qiniu.com/hub )
        private const string HubName = "";

        // Replace with your domain ( https://portal.qiniu.com/hub/{your_hub}/domain )
        private const string Domain = "example.com";

        private Client _cli;
        private Hub _hub;
        private const string Prefix = "SomeTest";
        private const string KeyA = Prefix + "A";
        private const string KeyB = Prefix + "B";

        private void Prepare()
        {
            Assert.NotNull(AccessKey);
            Assert.NotEqual("", AccessKey);
            Assert.NotNull(SecretKey);
            Assert.NotEqual("", SecretKey);
            Assert.NotNull(HubName);
            Assert.NotEqual("", HubName);
            _cli = new Client(AccessKey, SecretKey);
            _hub = _cli.NewHub(HubName);
        }

        [Fact]
        public void TestGetNoExistStream()
        {
            Prepare();
            try
            {
                _hub.Get(KeyA);
                Assert.True(false);
            }
            catch (PiliException e)
            {
                Assert.True(e.NotFound);
            }
        }

        [Fact]
        public void TestHistory()
        {
            Prepare();

            var key = Prefix + "History";
            try
            {
                var stream = _hub.Create(key);
                var records = stream.HistoryRecord(0, 0);
                Assert.Empty(records);
            }
            catch (PiliException)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void TestList()
        {
            Prepare();

            try
            {
                _hub.Create(KeyB + "1");
                _hub.Create(KeyB + "2");
            }
            catch (PiliException)
            {
                Assert.True(false);
            }

            try
            {
                var listRet = _hub.List(KeyB, 0, "");
                Assert.Equal(2, listRet.Keys.Length);
                Assert.Equal("", listRet.Omarker);
            }
            catch (PiliException)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void TestLive()
        {
            Prepare();

            try
            {
                var listRet = _hub.ListLive(Prefix, 0, "");
                Assert.Empty(listRet.Keys);
            }
            catch (PiliException)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void TestSave()
        {
            Prepare();

            var key = Prefix + "Save";
            try
            {
                var stream = _hub.Create(key);
                stream.Save(0, 0);
                Assert.True(false);
            }
            catch (PiliException e)
            {
                Assert.True(e.NotInLive);
            }
        }

        [Fact]
        public void TestStreamOperate()
        {
            Prepare();
            // create
            try
            {
                _hub.Create(KeyA);
            }
            catch (PiliException)
            {
                Assert.True(false);
            }

            // get
            Stream stream;
            try
            {
                stream = _hub.Get(KeyA);
                Assert.Equal(0, stream.DisabledTill);
                Assert.Equal(HubName, stream.Hub);
                Assert.Equal(KeyA, stream.Key);
            }
            catch (PiliException)
            {
                Assert.True(false);
            }

            // create again
            try
            {
                _hub.Create(KeyA);
                Assert.False(true);
            }
            catch (PiliException e)
            {
                Assert.True(e.Duplicate);
            }

            //disable
            try
            {
                stream = _hub.Get(KeyA);
                stream.Disable();
                stream = _hub.Get(KeyA);
                Assert.Equal(-1, stream.DisabledTill);
                Assert.Equal(HubName, stream.Hub);
                Assert.Equal(KeyA, stream.Key);
            }
            catch (PiliException)
            {
                Assert.True(false);
            }

            //enable
            try
            {
                stream = _hub.Get(KeyA);
                stream.Enable();
                stream.Info();
                Assert.Equal(0, stream.DisabledTill);
                Assert.Equal(HubName, stream.Hub);
                Assert.Equal(KeyA, stream.Key);
            }
            catch (PiliException)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void TestUpdateConverts()
        {
            Prepare();

            var key = Prefix + "Converts";
            try
            {
                var stream = _hub.Create(key);
                Assert.Null(stream.Converts);

                var profiles = new[] { "480p", "720p" };
                stream.UpdateConverts(profiles);
                stream = stream.Info();
                Assert.Equal(profiles, stream.Converts);
            }
            catch (PiliException)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void TestURL()
        {
            var key = Prefix + "TestURL";
            Prepare();
            var expect = "rtmp://pili-publish." + Domain + "/" + HubName + $"/{key}?e=";
            var url = _cli.RTMPPublishURL("pili-publish." + Domain, HubName, key, 3600);
            Assert.StartsWith(expect, url);

            expect = "rtmp://pili-live-rtmp." + Domain + "/" + HubName + $"/{key}";
            url = _cli.RTMPPlayURL("pili-live-rtmp." + Domain, HubName, key);
            Assert.StartsWith(expect, url);

            expect = "http://pili-live-hls." + Domain + "/" + HubName + $"/{key}.m3u8";
            url = _cli.HLSPlayURL("pili-live-hls." + Domain, HubName, key);
            Assert.StartsWith(expect, url);

            expect = "http://pili-live-hdl." + Domain + "/" + HubName + $"/{key}.flv";
            url = _cli.HDLPlayURL("pili-live-hdl." + Domain, HubName, key);
            Assert.StartsWith(expect, url);

            expect = "http://pili-live-snapshot." + Domain + "/" + HubName + $"/{key}.jpg";
            url = _cli.SnapshotPlayURL("pili-live-snapshot." + Domain, HubName, key);
            Assert.StartsWith(expect, url);
        }
    }
}
