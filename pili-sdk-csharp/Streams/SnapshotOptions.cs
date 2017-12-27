namespace Qiniu.Pili.Streams
{
    public class SnapshotOptions
    {
        public SnapshotOptions()
        {
        }

        public SnapshotOptions(string fname, long time, string format)
        {
            Fname = fname;
            Time = time;
            Format = format;
        }

        /// <summary>
        ///     the saved file name
        /// </summary>
        public string Fname { get; set; }

        /// <summary>
        ///     file format. default in jpg
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        ///     the unix time of snapshot. 0 means the current time
        /// </summary>
        public long Time { get; set; }
    }
}
