using System;
using Qiniu.Pili;
using Qiniu.Pili.Meetings;
using Xunit;

namespace pili_sdk_csharp
{
    public class MeetingTest
    {

        // Replace with your keys here ( https://portal.qiniu.com/user/key )
        private const string AccessKey = "";
        private const string SecretKey = "";


        // Replace with your hub name ( https://portal.qiniu.com/hub )
        private const string HubName = "";

        private const string RoomName = "test12Room";

        private Client _cli;
        private Meeting _meeting;

        private void Prepare()
        {
            Assert.NotNull(AccessKey);
            Assert.NotEqual("", AccessKey);
            Assert.NotNull(SecretKey);
            Assert.NotEqual("", SecretKey);
            Assert.NotNull(HubName);
            Assert.NotEqual("", HubName);
            _cli = new Client(AccessKey, SecretKey);
            _meeting = _cli.NewMeeting();
        }

        [Fact]
        public void TestCreateRoom()
        {
            Prepare();
            try
            {
                var r1 = _meeting.CreateRoom("123", RoomName, 12);
                Assert.Equal(RoomName, r1);

                var room = _meeting.GetRoom(RoomName);
                Console.WriteLine("roomName:" + room.Name);
                Console.WriteLine("roomStatus:" + room.Status);
                Assert.Equal(RoomName, room.Name);
                Assert.Equal("admin", room.OwnerId);
                Assert.Equal(Status.New, room.Status);
            }
            catch (PiliException e)
            {
                Console.WriteLine(e.StackTrace);
                Assert.True(false);
            }
        }

        [Fact]
        public void TestRoomDelete()
        {
            Prepare();
            try
            {
                _meeting.DeleteRoom(RoomName);
            }
            catch (PiliException e)
            {
                Console.WriteLine(e.StackTrace);
                Assert.True(false);
            }
        }

        [Fact]
        public void TestRoomToken()
        {
            Prepare();
            try
            {
                var token = _meeting.RoomToken("room1", "123", "admin", new DateTime(1785600000000L));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Assert.True(false);
            }
        }
    }
}
