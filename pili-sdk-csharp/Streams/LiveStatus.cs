namespace Qiniu.Pili.Streams
{
    public class LiveStatus
    {
        public int Bps { get; set; }

        public string ClientIp { get; set; }

        public FPSStatus FPS { get; set; }

        public long StartAt { get; set; }
    }
}
